using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.Repositories;
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Application.Rules;

public class BaseFeeRule : IPricingRule
{
    private readonly IPricingConfigRepository _repository;

    public BaseFeeRule(IPricingConfigRepository repository)
    {
        _repository = repository;
    }

    public int Priority => 1;
    public string RuleName => "BaseFee";
    public bool IsEnabled => true;

    public Task<bool> AppliesAsync(PricingContext context, CancellationToken ct = default)
    {
        return Task.FromResult(true); // Always applies
    }

    public async Task<PriceModification?> CalculateAsync(PricingContext context, CancellationToken ct = default)
    {
        var config = await _repository.GetActiveBaseFeeConfigAsync(ct);
        if (config == null) return null;

        return new PriceModification(
            RuleName,
            "Base delivery fee",
            config.Amount,
            ModificationType.Flat,
            Priority);
    }
}