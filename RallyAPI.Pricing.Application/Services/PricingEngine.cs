// RallyAPI.Pricing.Application/Services/PricingEngine.cs
using Microsoft.Extensions.Logging;
using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.Repositories;
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Application.Services;

public class PricingEngine : IPricingEngine
{
    private readonly IEnumerable<IPricingRule> _rules;
    private readonly IPricingConfigRepository _configRepository;
    private readonly ILogger<PricingEngine> _logger;
    private const int QuoteExpiryMinutes = 10;

    public PricingEngine(
        IEnumerable<IPricingRule> rules,
        IPricingConfigRepository configRepository,
        ILogger<PricingEngine> logger)
    {
        _rules = rules;
        _configRepository = configRepository;
        _logger = logger;
    }

    public async Task<PricingResult> CalculateDeliveryFeeAsync(
        PricingContext context,
        CancellationToken ct = default)
    {
        var modifications = new List<PriceModification>();
        var appliedMods = new List<AppliedModification>();

        // Run rules in priority order
        var orderedRules = _rules
            .Where(r => r.IsEnabled)
            .OrderBy(r => r.Priority);

        foreach (var rule in orderedRules)
        {
            try
            {
                if (!await rule.AppliesAsync(context, ct))
                    continue;

                var modification = await rule.CalculateAsync(context, ct);
                if (modification != null)
                {
                    modifications.Add(modification);
                    _logger.LogDebug("Rule {Rule} applied: {Amount} ({Type})",
                        rule.RuleName, modification.Amount, modification.Type);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing rule {Rule}", rule.RuleName);
            }
        }

        // Calculate final fee
        var (baseFee, finalFee, surgeMultiplier, surgeReason) = CalculateFinal(modifications);

        // Build breakdown
        foreach (var mod in modifications)
        {
            var appliedAmount = mod.Type == ModificationType.Flat
                ? mod.Amount
                : mod.Apply(baseFee);
            appliedMods.Add(new AppliedModification(mod.RuleName, mod.Description, appliedAmount));
        }

        // Apply min/max caps
        var config = await _configRepository.GetActiveBaseFeeConfigAsync(ct);
        if (config != null)
        {
            if (config.MinimumFee.HasValue && finalFee < config.MinimumFee.Value)
                finalFee = config.MinimumFee.Value;

            if (config.MaximumFee.HasValue && finalFee > config.MaximumFee.Value)
                finalFee = config.MaximumFee.Value;
        }

        // Generate quote ID
        var quoteId = GenerateQuoteId();
        var expiresAt = DateTime.UtcNow.AddMinutes(QuoteExpiryMinutes);

        return new PricingResult(
            quoteId,
            expiresAt,
            baseFee,
            finalFee,
            surgeMultiplier,
            surgeReason,
            context.ThirdPartyQuote,
            appliedMods);
    }

    private static string GenerateQuoteId()
    {
        return $"quote_{Guid.NewGuid():N}"[..24];
    }

    private (decimal baseFee, decimal finalFee, decimal surgeMultiplier, string? surgeReason)
        CalculateFinal(List<PriceModification> modifications)
    {
        decimal baseFee = 0;
        decimal flatTotal = 0;
        decimal multiplier = 1;
        string? surgeReason = null;

        foreach (var mod in modifications.OrderBy(m => m.Priority))
        {
            switch (mod.Type)
            {
                case ModificationType.Flat:
                    if (mod.RuleName == "BaseFee")
                        baseFee = mod.Amount;
                    else
                        flatTotal += mod.Amount;
                    break;

                case ModificationType.Percentage:
                    flatTotal += baseFee * (mod.Amount / 100);
                    break;

                case ModificationType.Multiplier:
                    multiplier *= mod.Amount;
                    if (surgeReason == null && mod.Amount > 1)
                        surgeReason = mod.Description;
                    break;
            }
        }

        var finalFee = (baseFee + flatTotal) * multiplier;

        return (baseFee, Math.Round(finalFee, 2), multiplier, surgeReason);
    }
}