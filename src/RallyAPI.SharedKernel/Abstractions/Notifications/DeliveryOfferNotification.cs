namespace RallyAPI.SharedKernel.Abstractions.Notifications;

/// <summary>
/// Notification payload sent to rider for a delivery offer.
/// </summary>
public sealed record DeliveryOfferNotification
{
    /// <summary>
    /// Unique offer identifier.
    /// </summary>
    public required Guid OfferId { get; init; }

    /// <summary>
    /// Delivery request this offer is for.
    /// </summary>
    public required Guid DeliveryRequestId { get; init; }

    /// <summary>
    /// Order number for display.
    /// </summary>
    public required string OrderNumber { get; init; }

    /// <summary>
    /// Restaurant name.
    /// </summary>
    public required string RestaurantName { get; init; }

    /// <summary>
    /// Pickup address (brief).
    /// </summary>
    public required string PickupAddress { get; init; }

    /// <summary>
    /// Pickup location.
    /// </summary>
    public required double PickupLatitude { get; init; }
    public required double PickupLongitude { get; init; }

    /// <summary>
    /// Drop address (brief).
    /// </summary>
    public required string DropAddress { get; init; }

    /// <summary>
    /// Drop location.
    /// </summary>
    public required double DropLatitude { get; init; }
    public required double DropLongitude { get; init; }

    /// <summary>
    /// Distance from rider's current location to restaurant (km).
    /// </summary>
    public required decimal DistanceToPickupKm { get; init; }

    /// <summary>
    /// Distance from restaurant to customer (km).
    /// </summary>
    public required decimal DistanceToDropKm { get; init; }

    /// <summary>
    /// Total trip distance (pickup + drop).
    /// </summary>
    public decimal TotalDistanceKm => DistanceToPickupKm + DistanceToDropKm;

    /// <summary>
    /// Rider earnings for this delivery.
    /// </summary>
    public required decimal Earnings { get; init; }

    /// <summary>
    /// Seconds remaining to accept this offer.
    /// </summary>
    public required int ExpiresInSeconds { get; init; }

    /// <summary>
    /// When this offer was created.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// When this offer expires.
    /// </summary>
    public required DateTime ExpiresAt { get; init; }

    /// <summary>
    /// Whether food is already ready for pickup.
    /// </summary>
    public required bool IsFoodReady { get; init; }

    /// <summary>
    /// Estimated time until food is ready (if not ready).
    /// </summary>
    public int? FoodReadyInMinutes { get; init; }
}