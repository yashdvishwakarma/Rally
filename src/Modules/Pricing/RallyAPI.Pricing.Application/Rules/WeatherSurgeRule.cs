using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.Repositories;
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Application.Rules;

public class WeatherSurgeRule : IPricingRule
{
    private readonly IPricingConfigRepository _repository;

    public WeatherSurgeRule(IPricingConfigRepository repository)
    {
        _repository = repository;
    }

    public int Priority => 4;
    public string RuleName => "WeatherSurge";
    public bool IsEnabled => true;

    public Task<bool> AppliesAsync(PricingContext context, CancellationToken ct = default)
    {
        // Only apply if weather is worse than clear
        return Task.FromResult(context.Weather.HasValue && context.Weather.Value > WeatherCondition.Cloudy);
    }

    public async Task<PriceModification?> CalculateAsync(PricingContext context, CancellationToken ct = default)
    {
        if (!context.Weather.HasValue) return null;

        var surge = await _repository.GetWeatherSurgeAsync(context.Weather.Value, ct);
        if (surge == null) return null;

        if (surge.Multiplier.HasValue)
        {
            return new PriceModification(
                RuleName,
                surge.Description,
                surge.Multiplier.Value,
                ModificationType.Multiplier,
                Priority);
        }

        return new PriceModification(
            RuleName,
            surge.Description,
            surge.SurgeAmount,
            ModificationType.Flat,
            Priority);
    }
}