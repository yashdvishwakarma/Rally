// RallyAPI.Pricing.Application/Rules/SpecialDayRule.cs
using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.Repositories;
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Application.Rules;

public class SpecialDayRule : IPricingRule
{
    private readonly IPricingConfigRepository _repository;

    public SpecialDayRule(IPricingConfigRepository repository)
    {
        _repository = repository;
    }

    public int Priority => 6;
    public string RuleName => "SpecialDay";
    public bool IsEnabled => true;

    public async Task<bool> AppliesAsync(PricingContext context, CancellationToken ct = default)
    {
        var date = DateOnly.FromDateTime(context.OrderTime);
        var surge = await _repository.GetSpecialDaySurgeAsync(date, ct);
        return surge != null;
    }

    public async Task<PriceModification?> CalculateAsync(PricingContext context, CancellationToken ct = default)
    {
        var date = DateOnly.FromDateTime(context.OrderTime);
        var surge = await _repository.GetSpecialDaySurgeAsync(date, ct);

        if (surge == null) return null;

        if (surge.Multiplier.HasValue)
        {
            return new PriceModification(
                RuleName,
                $"{surge.Reason} surge",
                surge.Multiplier.Value,
                ModificationType.Multiplier,
                Priority);
        }

        return new PriceModification(
            RuleName,
            $"{surge.Reason} surge",
            surge.SurgeAmount,
            ModificationType.Flat,
            Priority);
    }
}