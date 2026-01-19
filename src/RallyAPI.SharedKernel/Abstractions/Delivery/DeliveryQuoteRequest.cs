namespace RallyAPI.SharedKernel.Abstractions.Delivery;

/// <summary>
/// Request model for getting delivery quotes.
/// Contains all information needed to calculate delivery pricing.
/// </summary>
public sealed record DeliveryQuoteRequest
{
    public required double PickupLatitude { get; init; }
    public required double PickupLongitude { get; init; }
    public required string PickupPincode { get; init; }

    public required double DropLatitude { get; init; }
    public required double DropLongitude { get; init; }
    public required string DropPincode { get; init; }

    public required string City { get; init; }
    public required decimal OrderAmount { get; init; }

    /// <summary>
    /// Order weight in kilograms. Optional - defaults to provider config if not specified.
    /// </summary>
    public decimal? OrderWeight { get; init; }

    /// <summary>
    /// Creates a new delivery quote request with all required fields.
    /// </summary>
    public static DeliveryQuoteRequest Create(
        double pickupLatitude,
        double pickupLongitude,
        string pickupPincode,
        double dropLatitude,
        double dropLongitude,
        string dropPincode,
        string city,
        decimal orderAmount,
        decimal? orderWeight = null)
    {
        return new DeliveryQuoteRequest
        {
            PickupLatitude = pickupLatitude,
            PickupLongitude = pickupLongitude,
            PickupPincode = pickupPincode,
            DropLatitude = dropLatitude,
            DropLongitude = dropLongitude,
            DropPincode = dropPincode,
            City = city,
            OrderAmount = orderAmount,
            OrderWeight = orderWeight
        };
    }
}