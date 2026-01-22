namespace RallyAPI.Pricing.Application.Abstractions;

public interface IDemandTracker
{
    Task<int> GetCurrentOrdersPerHourAsync(
        Guid? restaurantId = null,
        CancellationToken ct = default);
}