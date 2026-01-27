namespace RallyAPI.SharedKernel.Abstractions.Pricing;

/// <summary>
/// Request for calculating own fleet delivery price.
/// </summary>
public sealed record DeliveryPriceRequest
{
    /// <summary>
    /// Restaurant/pickup location latitude.
    /// </summary>
    public required double PickupLatitude { get; init; }

    /// <summary>
    /// Restaurant/pickup location longitude.
    /// </summary>
    public required double PickupLongitude { get; init; }

    /// <summary>
    /// Customer/drop location latitude.
    /// </summary>
    public required double DropLatitude { get; init; }

    /// <summary>
    /// Customer/drop location longitude.
    /// </summary>
    public required double DropLongitude { get; init; }

    /// <summary>
    /// City name for zone-based pricing (future).
    /// </summary>
    public required string City { get; init; }

    /// <summary>
    /// Order subtotal (food cost) - may affect pricing rules.
    /// </summary>
    public required decimal OrderAmount { get; init; }

    /// <summary>
    /// Restaurant ID for restaurant-specific pricing rules (future).
    /// </summary>
    public Guid? RestaurantId { get; init; }

    /// <summary>
    /// For future scheduled delivery support.
    /// Null means immediate delivery.
    /// </summary>
    public DateTime? ScheduledTime { get; init; }
}