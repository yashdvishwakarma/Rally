// RallyAPI.Pricing.Domain/ValueObjects/PricingContext.cs
using RallyAPI.Pricing.Domain.Enums;

namespace RallyAPI.Pricing.Domain.ValueObjects;

public class PricingContext
{
    // Location
    public double RestaurantLatitude { get; init; }
    public double RestaurantLongitude { get; init; }
    public double CustomerLatitude { get; init; }
    public double CustomerLongitude { get; init; }

    // Pincodes (needed for 3PL)
    public string? PickupPincode { get; init; }
    public string? DropPincode { get; init; }
    public string? City { get; init; }

    // Time
    public DateTime OrderTime { get; init; }
    public DayOfWeek DayOfWeek { get; init; }

    // Order Info
    public decimal OrderSubtotal { get; init; }
    public int ItemCount { get; init; }
    public decimal? OrderWeight { get; init; }
    public Guid RestaurantId { get; init; }
    public Guid? CustomerId { get; init; }

    // External Factors
    public WeatherCondition? Weather { get; init; }
    public int? CurrentOrdersPerHour { get; init; }

    // Optional
    public string? PromoCode { get; init; }

    // 3PL Quote (set by rule)
    public DeliveryQuote? ThirdPartyQuote { get; private set; }

    // Calculated
    public double DistanceKm => CalculateDistance();

    public void SetThirdPartyQuote(
        string quoteId,
        string providerName,
        decimal price,
        int estimatedMinutes)
    {
        ThirdPartyQuote = DeliveryQuote.CreateWithExpiry(
            quoteId,
            providerName,
            price,
            estimatedMinutes);
    }

    private double CalculateDistance()
    {
        const double EarthRadiusKm = 6371;

        var dLat = ToRadians(CustomerLatitude - RestaurantLatitude);
        var dLon = ToRadians(CustomerLongitude - RestaurantLongitude);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(RestaurantLatitude)) *
                Math.Cos(ToRadians(CustomerLatitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}