// RallyAPI.Pricing.Application/Abstractions/IWeatherProvider.cs
using RallyAPI.Pricing.Domain.Enums;

namespace RallyAPI.Pricing.Application.Abstractions;

public interface IWeatherProvider
{
    Task<WeatherCondition> GetCurrentWeatherAsync(
        double latitude,
        double longitude,
        CancellationToken ct = default);
}