namespace RallyAPI.SharedKernel.Abstractions.Delivery;

/// <summary>
/// Result of creating a delivery task with a third-party provider.
/// </summary>
public sealed record CreateTaskResult
{
    private CreateTaskResult() { }

    public bool IsSuccess { get; private init; }

    /// <summary>
    /// Task ID from the provider (for tracking and cancellation).
    /// </summary>
    public string? TaskId { get; private init; }

    /// <summary>
    /// Our order ID echoed back.
    /// </summary>
    public string? ClientOrderId { get; private init; }

    /// <summary>
    /// Initial task state from provider.
    /// </summary>
    public string? State { get; private init; }

    /// <summary>
    /// Tracking URL if available.
    /// </summary>
    public string? TrackingUrl { get; private init; }

    /// <summary>
    /// Error message if failed.
    /// </summary>
    public string? ErrorMessage { get; private init; }

    /// <summary>
    /// Provider name.
    /// </summary>
    public string? ProviderName { get; private init; }

    public static CreateTaskResult Success(
        string taskId,
        string clientOrderId,
        string state,
        string? trackingUrl,
        string providerName)
    {
        return new CreateTaskResult
        {
            IsSuccess = true,
            TaskId = taskId,
            ClientOrderId = clientOrderId,
            State = state,
            TrackingUrl = trackingUrl,
            ProviderName = providerName
        };
    }

    public static CreateTaskResult Failure(string errorMessage, string providerName)
    {
        return new CreateTaskResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ProviderName = providerName
        };
    }
}