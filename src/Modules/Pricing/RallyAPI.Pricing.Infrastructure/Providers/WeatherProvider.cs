// RallyAPI.Pricing.Infrastructure/Providers/WeatherProvider.cs
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RallyAPI.Pricing.Application.Abstractions;
using RallyAPI.Pricing.Domain.Enums;

namespace RallyAPI.Pricing.Infrastructure.Providers;

public class WeatherProvider : IWeatherProvider
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<WeatherProvider> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    public WeatherProvider(
        HttpClient httpClient,
        IMemoryCache cache,
        ILogger<WeatherProvider> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<WeatherCondition> GetCurrentWeatherAsync(
        double latitude,
        double longitude,
        CancellationToken ct = default)
    {
        var cacheKey = $"weather:{latitude:F2}:{longitude:F2}";

        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;

            try
            {
                // TODO: Integrate with actual weather API (OpenWeatherMap, etc.)
                // For now, return Clear as default
                // 
                // Example integration:
                // var response = await _httpClient.GetAsync(
                //     $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}");
                // var data = await response.Content.ReadFromJsonAsync<WeatherResponse>();
                // return MapToCondition(data.Weather.Main);

                return WeatherCondition.Clear;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get weather data");
                return WeatherCondition.Clear; // Safe default
            }
        });
    }
}