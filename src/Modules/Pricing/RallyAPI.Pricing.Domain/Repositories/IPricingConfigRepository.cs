using RallyAPI.Pricing.Domain.Entities;
using RallyAPI.Pricing.Domain.Enums;

namespace RallyAPI.Pricing.Domain.Repositories;

public interface IPricingConfigRepository
{
    // Base Fee
    Task<BaseFeeConfig?> GetActiveBaseFeeConfigAsync(CancellationToken ct = default);

    // Distance
    Task<IReadOnlyList<DistanceRate>> GetActiveDistanceRatesAsync(CancellationToken ct = default);

    // Time Surge
    Task<IReadOnlyList<TimeSurge>> GetActiveTimeSurgesAsync(CancellationToken ct = default);

    // Weather Surge
    Task<WeatherSurge?> GetWeatherSurgeAsync(WeatherCondition condition, CancellationToken ct = default);

    // Demand Surge
    Task<IReadOnlyList<DemandSurge>> GetActiveDemandSurgesAsync(CancellationToken ct = default);

    // Special Days
    Task<SpecialDaySurge?> GetSpecialDaySurgeAsync(DateOnly date, CancellationToken ct = default);
}