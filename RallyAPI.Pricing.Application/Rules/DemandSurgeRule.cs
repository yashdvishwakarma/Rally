// RallyAPI.Pricing.Application/Rules/DemandSurgeRule.cs
using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.Repositories;
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Application.Rules;

public class DemandSurgeRule : IPricingRule
{
    private readonly IPricingConfigRepository _repository;

    public DemandSurgeRule(IPricingConfigRepository repository)
    {
        _repository = repository;
    }

    public int Priority => 5;
    public string RuleName => "DemandSurge";
    public bool IsEnabled => true;

    public Task<bool> AppliesAsync(PricingContext context, CancellationToken ct = default)
    {
        return Task.FromResult(context.CurrentOrdersPerHour.HasValue);
    }

    public async Task<PriceModification?> CalculateAsync(PricingContext context, CancellationToken ct = default)
    {
        if (!context.CurrentOrdersPerHour.HasValue) return null;

        var surges = await _repository.GetActiveDemandSurgesAsync(ct);
        var applicable = surges.FirstOrDefault(s => s.Applies(context.CurrentOrdersPerHour.Value));

        if (applicable == null) return null;

        return new PriceModification(
            RuleName,
            applicable.Description,
            applicable.Multiplier,
            ModificationType.Multiplier,
            Priority);
    }
}