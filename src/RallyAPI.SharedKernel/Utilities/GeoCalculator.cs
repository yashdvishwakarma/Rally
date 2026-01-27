namespace RallyAPI.SharedKernel.Utilities;

/// <summary>
/// Utility class for geographic calculations.
/// </summary>
public static class GeoCalculator
{
    private const double EarthRadiusKm = 6371.0;

    /// <summary>
    /// Calculates the distance between two points using the Haversine formula.
    /// </summary>
    /// <param name="lat1">Latitude of point 1</param>
    /// <param name="lon1">Longitude of point 1</param>
    /// <param name="lat2">Latitude of point 2</param>
    /// <param name="lon2">Longitude of point 2</param>
    /// <returns>Distance in kilometers</returns>
    public static double CalculateDistanceKm(
        double lat1, double lon1,
        double lat2, double lon2)
    {
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    /// <summary>
    /// Calculates the distance between two points using decimal coordinates.
    /// </summary>
    public static double CalculateDistanceKm(
        decimal lat1, decimal lon1,
        decimal lat2, decimal lon2)
    {
        return CalculateDistanceKm(
            (double)lat1, (double)lon1,
            (double)lat2, (double)lon2);
    }

    /// <summary>
    /// Checks if a point is within a radius of another point.
    /// </summary>
    /// <param name="centerLat">Center point latitude</param>
    /// <param name="centerLon">Center point longitude</param>
    /// <param name="pointLat">Point to check latitude</param>
    /// <param name="pointLon">Point to check longitude</param>
    /// <param name="radiusKm">Radius in kilometers</param>
    /// <returns>True if point is within radius</returns>
    public static bool IsWithinRadius(
        double centerLat, double centerLon,
        double pointLat, double pointLon,
        double radiusKm)
    {
        var distance = CalculateDistanceKm(centerLat, centerLon, pointLat, pointLon);
        return distance <= radiusKm;
    }

    /// <summary>
    /// Calculates a bounding box for initial filtering (faster than Haversine for large datasets).
    /// </summary>
    /// <param name="lat">Center latitude</param>
    /// <param name="lon">Center longitude</param>
    /// <param name="radiusKm">Radius in kilometers</param>
    /// <returns>Bounding box coordinates</returns>
    public static BoundingBox GetBoundingBox(double lat, double lon, double radiusKm)
    {
        // Rough approximation: 1 degree latitude ≈ 111 km
        // Longitude varies with latitude
        var latDelta = radiusKm / 111.0;
        var lonDelta = radiusKm / (111.0 * Math.Cos(ToRadians(lat)));

        return new BoundingBox
        {
            MinLatitude = lat - latDelta,
            MaxLatitude = lat + latDelta,
            MinLongitude = lon - lonDelta,
            MaxLongitude = lon + lonDelta
        };
    }

    /// <summary>
    /// Estimates travel time based on distance.
    /// </summary>
    /// <param name="distanceKm">Distance in kilometers</param>
    /// <param name="averageSpeedKmh">Average speed in km/h (default 20 for city bike)</param>
    /// <returns>Estimated minutes</returns>
    public static int EstimateTravelMinutes(double distanceKm, double averageSpeedKmh = 20.0)
    {
        if (distanceKm <= 0) return 0;

        var hours = distanceKm / averageSpeedKmh;
        var minutes = (int)Math.Ceiling(hours * 60);

        // Add buffer for traffic, stops, etc.
        return Math.Max(5, minutes + 2);
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}

/// <summary>
/// Represents a geographic bounding box.
/// </summary>
public sealed record BoundingBox
{
    public double MinLatitude { get; init; }
    public double MaxLatitude { get; init; }
    public double MinLongitude { get; init; }
    public double MaxLongitude { get; init; }
}