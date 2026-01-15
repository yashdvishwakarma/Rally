using RallyAPI.Pricing.Domain.Enums;

namespace RallyAPI.Pricing.Domain.ValueObjects;

public record PricingContext(
    // Location
    double RestaurantLatitude,
    double RestaurantLongitude,
    double CustomerLatitude,
    double CustomerLongitude,

    // Time
    DateTime OrderTime,
    DayOfWeek DayOfWeek,

    // Order Info
    decimal OrderSubtotal,
    int ItemCount,
    Guid RestaurantId,
    Guid? CustomerId,

    // External Factors
    WeatherCondition? Weather,
    int? CurrentOrdersPerHour,  // For demand calculation

    // Optional
    string? PromoCode)
{
    // Calculated property
    public double DistanceKm => CalculateDistance();

    private double CalculateDistance()
    {
        const double EarthRadiusKm = 6371;

        var dLat = ToRadians(CustomerLatitude - RestaurantLatitude);
        var dLon = ToRadians(CustomerLongitude - RestaurantLongitude);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(RestaurantLatitude)) * Math.Cos(ToRadians(CustomerLatitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}