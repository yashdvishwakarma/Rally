namespace RallyAPI.SharedKernel.Abstractions.Riders;

/// <summary>
/// Detailed rider information for display and tracking purposes.
/// </summary>
public sealed record RiderDetails
{
    public required Guid RiderId { get; init; }
    public required string Name { get; init; }
    public required string Phone { get; init; }
    public string? VehicleNumber { get; init; }
    public required string VehicleType { get; init; }
    public required bool IsOnline { get; init; }
    public required bool IsActive { get; init; }

    /// <summary>
    /// Current delivery assignment, if any.
    /// Null means rider is free.
    /// </summary>
    public Guid? CurrentDeliveryId { get; init; }

    /// <summary>
    /// Current location (if online and location available).
    /// </summary>
    public double? CurrentLatitude { get; init; }
    public double? CurrentLongitude { get; init; }
    public DateTime? LastLocationUpdate { get; init; }
}