using RallyAPI.SharedKernel.Results;

namespace RallyAPI.SharedKernel.Abstractions.Riders;

/// <summary>
/// Service for modifying rider state related to deliveries.
/// Implemented by Users module, consumed by Delivery module.
/// </summary>
public interface IRiderCommandService
{
    /// <summary>
    /// Assigns a delivery to a rider.
    /// Sets Rider.CurrentDeliveryId to the specified delivery.
    /// </summary>
    /// <param name="riderId">Rider to assign</param>
    /// <param name="deliveryRequestId">Delivery request being assigned</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Success or failure with error</returns>
    Task<Result> AssignDeliveryToRiderAsync(
        Guid riderId,
        Guid deliveryRequestId,
        CancellationToken ct = default);

    /// <summary>
    /// Clears the rider's current delivery assignment.
    /// Called when delivery is completed, failed, or reassigned.
    /// </summary>
    /// <param name="riderId">Rider ID</param>
    /// <param name="deliveryRequestId">Expected current delivery (for validation)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Success or failure with error</returns>
    Task<Result> ClearRiderDeliveryAsync(
        Guid riderId,
        Guid deliveryRequestId,
        CancellationToken ct = default);

    /// <summary>
    /// Updates rider's current location.
    /// Called by rider app periodically.
    /// </summary>
    /// <param name="riderId">Rider ID</param>
    /// <param name="latitude">Current latitude</param>
    /// <param name="longitude">Current longitude</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Success or failure with error</returns>
    Task<Result> UpdateRiderLocationAsync(
        Guid riderId,
        double latitude,
        double longitude,
        CancellationToken ct = default);
}