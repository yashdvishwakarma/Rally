namespace RallyAPI.Orders.Application.DTOs;

/// <summary>
/// Delivery information data transfer object.
/// </summary>
public sealed record DeliveryInfoDto
{
    // Pickup
    public double PickupLatitude { get; init; }
    public double PickupLongitude { get; init; }
    public string PickupPincode { get; init; } = string.Empty;
    public string? PickupAddress { get; init; }

    // Drop
    public AddressDto DeliveryAddress { get; init; } = new();

    // Quote
    public string? QuoteId { get; init; }
    public string? ProviderName { get; init; }
    public decimal? QuotedDeliveryFee { get; init; }
    public int? EstimatedMinutes { get; init; }
    public DateTime? QuotedAt { get; init; }

    // Rider
    public Guid? RiderId { get; init; }
    public string? RiderName { get; init; }
    public string? RiderPhone { get; init; }
    public string? TrackingUrl { get; init; }

    // Timestamps
    public DateTime? AssignedAt { get; init; }
    public DateTime? PickedUpAt { get; init; }
    public DateTime? DeliveredAt { get; init; }

    // Distance
    public double? DistanceKm { get; init; }
    public string? DistanceDisplay { get; init; }
    public string? EstimatedTimeDisplay { get; init; }
}