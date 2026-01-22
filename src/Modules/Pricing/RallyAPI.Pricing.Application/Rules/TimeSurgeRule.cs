// RallyAPI.Pricing.Application/Rules/TimeSurgeRule.cs
using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.Repositories;
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Application.Rules;

public class TimeSurgeRule : IPricingRule
{
    private readonly IPricingConfigRepository _repository;

    public TimeSurgeRule(IPricingConfigRepository repository)
    {
        _repository = repository;
    }

    public int Priority => 3;
    public string RuleName => "TimeSurge";
    public bool IsEnabled => true;

    public async Task<bool> AppliesAsync(PricingContext context, CancellationToken ct = default)
    {
        var surges = await _repository.GetActiveTimeSurgesAsync(ct);
        return surges.Any(s => s.Applies(context.OrderTime));
    }

    public async Task<PriceModification?> CalculateAsync(PricingContext context, CancellationToken ct = default)
    {
        var surges = await _repository.GetActiveTimeSurgesAsync(ct);
        var applicable = surges.FirstOrDefault(s => s.Applies(context.OrderTime));

        if (applicable == null) return null;

        return new PriceModification(
            RuleName,
            applicable.Description,
            applicable.SurgeAmount,
            ModificationType.Flat,
            Priority);
    }
}