// RallyAPI.Pricing.Infrastructure/Repositories/PricingConfigRepository.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RallyAPI.Pricing.Domain.Entities;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.Repositories;
using RallyAPI.Pricing.Infrastructure.Persistence;

namespace RallyAPI.Pricing.Infrastructure.Repositories;

public class PricingConfigRepository : IPricingConfigRepository
{
    private readonly PricingDbContext _context;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public PricingConfigRepository(PricingDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<BaseFeeConfig?> GetActiveBaseFeeConfigAsync(CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync("pricing:base_fee", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.BaseFeeConfigs
                .Where(x => x.IsActive)
                .FirstOrDefaultAsync(ct);
        });
    }

    public async Task<IReadOnlyList<DistanceRate>> GetActiveDistanceRatesAsync(CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync("pricing:distance_rates", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.DistanceRates
                .Where(x => x.IsActive)
                .OrderBy(x => x.MinDistanceKm)
                .ToListAsync(ct);
        }) ?? new List<DistanceRate>();
    }

    public async Task<IReadOnlyList<TimeSurge>> GetActiveTimeSurgesAsync(CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync("pricing:time_surges", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.TimeSurges
                .Where(x => x.IsActive)
                .ToListAsync(ct);
        }) ?? new List<TimeSurge>();
    }

    public async Task<WeatherSurge?> GetWeatherSurgeAsync(WeatherCondition condition, CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync($"pricing:weather:{condition}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.WeatherSurges
                .Where(x => x.IsActive && x.Condition == condition)
                .FirstOrDefaultAsync(ct);
        });
    }

    public async Task<IReadOnlyList<DemandSurge>> GetActiveDemandSurgesAsync(CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync("pricing:demand_surges", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.DemandSurges
                .Where(x => x.IsActive)
                .OrderBy(x => x.MinOrdersPerHour)
                .ToListAsync(ct);
        }) ?? new List<DemandSurge>();
    }

    public async Task<SpecialDaySurge?> GetSpecialDaySurgeAsync(DateOnly date, CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync($"pricing:special_day:{date}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.SpecialDaySurges
                .Where(x => x.IsActive && x.Date == date)
                .FirstOrDefaultAsync(ct);
        });
    }
}