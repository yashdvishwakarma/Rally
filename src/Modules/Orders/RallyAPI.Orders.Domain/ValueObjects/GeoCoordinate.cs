using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.ValueObjects;

/// <summary>
/// Represents a geographic coordinate (latitude/longitude).
/// Used for pickup and delivery locations.
/// </summary>
public sealed class GeoCoordinate : ValueObject
{
    public double Latitude { get; }
    public double Longitude { get; }

    private GeoCoordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static GeoCoordinate Create(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90", nameof(latitude));

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180", nameof(longitude));

        return new GeoCoordinate(latitude, longitude);
    }

    /// <summary>
    /// Calculates distance to another coordinate in kilometers (Haversine formula)
    /// </summary>
    public double DistanceTo(GeoCoordinate other)
    {
        const double EarthRadiusKm = 6371;

        var dLat = ToRadians(other.Latitude - Latitude);
        var dLon = ToRadians(other.Longitude - Longitude);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(Latitude)) * Math.Cos(ToRadians(other.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }

    public override string ToString() => $"({Latitude:F6}, {Longitude:F6})";
}