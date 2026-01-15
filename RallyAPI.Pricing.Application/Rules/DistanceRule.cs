// RallyAPI.Pricing.Application/Rules/DistanceRule.cs
using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.Repositories;
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Application.Rules;

public class DistanceRule : IPricingRule
{
    private readonly IPricingConfigRepository _repository;

    public DistanceRule(IPricingConfigRepository repository)
    {
        _repository = repository;
    }

    public int Priority => 2;
    public string RuleName => "Distance";
    public bool IsEnabled => true;

    public Task<bool> AppliesAsync(PricingContext context, CancellationToken ct = default)
    {
        return Task.FromResult(context.DistanceKm > 0);
    }

    public async Task<PriceModification?> CalculateAsync(PricingContext context, CancellationToken ct = default)
    {
        var rates = await _repository.GetActiveDistanceRatesAsync(ct);
        var applicableRate = rates.FirstOrDefault(r => r.IsInRange(context.DistanceKm));

        if (applicableRate == null) return null;

        return new PriceModification(
            RuleName,
            $"Distance charge ({context.DistanceKm:F1} km)",
            applicableRate.Rate,
            ModificationType.Flat,
            Priority);
    }
}