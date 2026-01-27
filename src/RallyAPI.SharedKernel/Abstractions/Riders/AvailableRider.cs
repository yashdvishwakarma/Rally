namespace RallyAPI.SharedKernel.Abstractions.Riders;

/// <summary>
/// Represents a rider available for delivery assignment.
/// Used when querying for nearby available riders.
/// </summary>
public sealed record AvailableRider
{
    public required Guid RiderId { get; init; }
    public required string Name { get; init; }
    public required string Phone { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }

    /// <summary>
    /// Distance from the pickup location in kilometers.
    /// </summary>
    public required double DistanceToPickupKm { get; init; }

    /// <summary>
    /// Vehicle type (Bike, Scooter, etc.)
    /// </summary>
    public required string VehicleType { get; init; }

    /// <summary>
    /// When the rider's location was last updated.
    /// Used to filter out stale locations.
    /// </summary>
    public required DateTime LocationUpdatedAt { get; init; }
}