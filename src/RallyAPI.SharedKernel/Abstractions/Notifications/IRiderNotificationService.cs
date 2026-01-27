using RallyAPI.SharedKernel.Results;

namespace RallyAPI.SharedKernel.Abstractions.Notifications;

/// <summary>
/// Service for sending real-time notifications to riders.
/// MVP implementation uses SignalR.
/// </summary>
public interface IRiderNotificationService
{
    /// <summary>
    /// Sends a delivery offer notification to a specific rider.
    /// </summary>
    /// <param name="riderId">Target rider ID</param>
    /// <param name="offer">Delivery offer details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Success if notification sent</returns>
    Task<Result> SendDeliveryOfferAsync(
        Guid riderId,
        DeliveryOfferNotification offer,
        CancellationToken ct = default);

    /// <summary>
    /// Notifies rider that their offer has been cancelled/expired.
    /// </summary>
    /// <param name="riderId">Target rider ID</param>
    /// <param name="offerId">Offer that was cancelled</param>
    /// <param name="reason">Reason for cancellation</param>
    /// <param name="ct">Cancellation token</param>
    Task<Result> SendOfferCancelledAsync(
        Guid riderId,
        Guid offerId,
        string reason,
        CancellationToken ct = default);

    /// <summary>
    /// Sends a general notification to rider.
    /// </summary>
    /// <param name="riderId">Target rider ID</param>
    /// <param name="title">Notification title</param>
    /// <param name="message">Notification message</param>
    /// <param name="ct">Cancellation token</param>
    Task<Result> SendNotificationAsync(
        Guid riderId,
        string title,
        string message,
        CancellationToken ct = default);

    /// <summary>
    /// Checks if rider is currently connected (online in app).
    /// </summary>
    /// <param name="riderId">Rider ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if rider is connected</returns>
    Task<bool> IsRiderConnectedAsync(
        Guid riderId,
        CancellationToken ct = default);
}