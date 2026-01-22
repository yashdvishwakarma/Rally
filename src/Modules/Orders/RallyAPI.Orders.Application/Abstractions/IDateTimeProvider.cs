namespace RallyAPI.Orders.Application.Abstractions;

/// <summary>
/// Abstraction for date/time operations.
/// Enables testing with controlled time.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateTimeOffset UtcNowOffset { get; }
}