using Microsoft.Extensions.Logging;
using RallyAPI.Orders.Domain.Abstractions;
using RallyAPI.Orders.Domain.Errors;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Infrastructure.Services;

/// <summary>
/// Implementation of order validation service.
/// For MVP: Minimal validation. Expand as modules integrate.
/// </summary>
public sealed class OrderValidationService : IOrderValidationService
{
    private readonly ILogger<OrderValidationService> _logger;

    // Configuration - can be moved to appsettings
    private static readonly TimeSpan DefaultOpeningTime = TimeSpan.FromHours(8);  // 8 AM
    private static readonly TimeSpan DefaultClosingTime = TimeSpan.FromHours(23); // 11 PM
    private const double MaxDeliveryDistanceKm = 15.0;

    public OrderValidationService(ILogger<OrderValidationService> logger)
    {
        _logger = logger;
    }

    public Task<Result> ValidateCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        // MVP: Just check ID is not empty
        // TODO: Integrate with Users module to verify customer exists
        if (customerId == Guid.Empty)
        {
            return Task.FromResult(Result.Failure(OrderErrors.InvalidCustomer(customerId)));
        }

        _logger.LogDebug("Customer {CustomerId} validated (MVP - ID check only)", customerId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ValidateRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        // MVP: Just check ID is not empty
        // TODO: Integrate with Catalog module to verify restaurant exists and is active
        if (restaurantId == Guid.Empty)
        {
            return Task.FromResult(Result.Failure(OrderErrors.InvalidRestaurant(restaurantId)));
        }

        _logger.LogDebug("Restaurant {RestaurantId} validated (MVP - ID check only)", restaurantId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ValidateRestaurantHoursAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        // MVP: Check against default operating hours
        // TODO: Integrate with Catalog module to get actual restaurant hours
        var now = DateTime.UtcNow.TimeOfDay;

        // Add IST offset for India (UTC+5:30)
        var istOffset = TimeSpan.FromHours(5.5);
        var istTime = now + istOffset;
        if (istTime >= TimeSpan.FromHours(24))
            istTime -= TimeSpan.FromHours(24);

        if (istTime < DefaultOpeningTime || istTime > DefaultClosingTime)
        {
            _logger.LogWarning(
                "Restaurant {RestaurantId} is outside operating hours. Current IST: {Time}",
                restaurantId,
                istTime);
            return Task.FromResult(Result.Failure(OrderErrors.RestaurantClosed));
        }

        _logger.LogDebug("Restaurant {RestaurantId} hours validated (MVP - default hours)", restaurantId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ValidateMenuItemsAsync(
        Guid restaurantId,
        IEnumerable<Guid> menuItemIds,
        CancellationToken cancellationToken = default)
    {
        // MVP: Just check IDs are not empty
        // TODO: Integrate with Catalog module to verify items exist and are available
        foreach (var itemId in menuItemIds)
        {
            if (itemId == Guid.Empty)
            {
                return Task.FromResult(Result.Failure(OrderErrors.InvalidMenuItem(itemId)));
            }
        }

        _logger.LogDebug("Menu items validated (MVP - ID check only)");
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ValidateDeliveryAddressAsync(
        double latitude,
        double longitude,
        string pincode,
        CancellationToken cancellationToken = default)
    {
        // MVP: Basic coordinate validation
        // TODO: Add service area validation, blacklist check, etc.

        if (latitude < -90 || latitude > 90)
        {
            return Task.FromResult(Result.Failure(
                Error.Validation("Invalid latitude value")));
        }

        if (longitude < -180 || longitude > 180)
        {
            return Task.FromResult(Result.Failure(
                Error.Validation("Invalid longitude value")));
        }

        if (string.IsNullOrWhiteSpace(pincode))
        {
            return Task.FromResult(Result.Failure(
                Error.Validation("Pincode is required")));
        }

        _logger.LogDebug("Delivery address validated (MVP - coordinate check only)");
        return Task.FromResult(Result.Success());
    }
}