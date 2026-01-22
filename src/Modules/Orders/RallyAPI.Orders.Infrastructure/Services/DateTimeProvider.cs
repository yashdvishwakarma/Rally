using RallyAPI.Orders.Application.Abstractions;

namespace RallyAPI.Orders.Infrastructure.Services;

/// <summary>
/// Default implementation of date/time provider.
/// </summary>
public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
}