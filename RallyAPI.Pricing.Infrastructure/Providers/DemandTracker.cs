// RallyAPI.Pricing.Infrastructure/Providers/DemandTracker.cs
using Microsoft.Extensions.Caching.Memory;
using RallyAPI.Pricing.Application.Abstractions;

namespace RallyAPI.Pricing.Infrastructure.Providers;

public class DemandTracker : IDemandTracker
{
    private readonly IMemoryCache _cache;
    private readonly string _connectionString;

    public DemandTracker(IMemoryCache cache, string connectionString)
    {
        _cache = cache;
        _connectionString = connectionString;
    }

    public async Task<int> GetCurrentOrdersPerHourAsync(
        Guid? restaurantId = null,
        CancellationToken ct = default)
    {
        var cacheKey = restaurantId.HasValue
            ? $"demand:restaurant:{restaurantId}"
            : "demand:global";

        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);

            try
            {
                using var connection = new Npgsql.NpgsqlConnection(_connectionString);
                await connection.OpenAsync(ct);

                var sql = restaurantId.HasValue
                    ? $@"SELECT COUNT(*) FROM orders.orders 
                         WHERE restaurant_id = '{restaurantId}' 
                         AND ordered_at > NOW() - INTERVAL '1 hour'"
                    : @"SELECT COUNT(*) FROM orders.orders 
                        WHERE ordered_at > NOW() - INTERVAL '1 hour'";

                using var command = new Npgsql.NpgsqlCommand(sql, connection);
                var result = await command.ExecuteScalarAsync(ct);

                return Convert.ToInt32(result);
            }
            catch
            {
                return 0; // Safe default
            }
        });
    }
}