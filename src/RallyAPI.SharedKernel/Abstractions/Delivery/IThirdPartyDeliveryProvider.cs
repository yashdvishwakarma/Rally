namespace RallyAPI.SharedKernel.Abstractions.Delivery;

/// <summary>
/// Extended interface for third-party delivery providers that support
/// full delivery lifecycle (quotes, booking, cancellation).
/// Extends the existing IDeliveryQuoteProvider.
/// </summary>
public interface IThirdPartyDeliveryProvider : IDeliveryQuoteProvider
{
    /// <summary>
    /// Gets quotes from multiple LSPs through this provider.
    /// </summary>
    /// <param name="request">Quote request with pickup/drop details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of available LSP quotes</returns>
    Task<ThirdPartyQuotesResult> GetQuotesAsync(
        DeliveryQuoteRequest request,
        CancellationToken ct = default);

    /// <summary>
    /// Creates/books a delivery task with the provider.
    /// </summary>
    /// <param name="request">Task creation request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created task details</returns>
    Task<CreateTaskResult> CreateTaskAsync(
        CreateTaskRequest request,
        CancellationToken ct = default);

    /// <summary>
    /// Cancels a previously created task.
    /// May fail if rider already picked up.
    /// </summary>
    /// <param name="taskId">Task ID from CreateTaskResult</param>
    /// <param name="reason">Cancellation reason</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Success or failure</returns>
    Task<CancelTaskResult> CancelTaskAsync(
        string taskId,
        string reason,
        CancellationToken ct = default);

    /// <summary>
    /// Gets current status of a task (fallback if webhook missed).
    /// </summary>
    /// <param name="taskId">Task ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Current task status</returns>
    Task<TaskStatusResult> GetTaskStatusAsync(
        string taskId,
        CancellationToken ct = default);
}

/// <summary>
/// Result of cancelling a task.
/// </summary>
public sealed record CancelTaskResult
{
    private CancelTaskResult() { }

    public bool IsSuccess { get; private init; }
    public string? ErrorMessage { get; private init; }
    public string? ProviderName { get; private init; }

    public static CancelTaskResult Success(string providerName) =>
        new() { IsSuccess = true, ProviderName = providerName };

    public static CancelTaskResult Failure(string errorMessage, string providerName) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage, ProviderName = providerName };
}

/// <summary>
/// Result of getting task status.
/// </summary>
public sealed record TaskStatusResult
{
    private TaskStatusResult() { }

    public bool IsSuccess { get; private init; }
    public string? TaskId { get; private init; }
    public string? State { get; private init; }
    public string? RiderName { get; private init; }
    public string? RiderPhone { get; private init; }
    public string? TrackingUrl { get; private init; }
    public string? ErrorMessage { get; private init; }
    public string? ProviderName { get; private init; }
    public DateTime? UpdatedAt { get; private init; }

    public static TaskStatusResult Success(
        string taskId,
        string state,
        string? riderName,
        string? riderPhone,
        string? trackingUrl,
        string providerName)
    {
        return new TaskStatusResult
        {
            IsSuccess = true,
            TaskId = taskId,
            State = state,
            RiderName = riderName,
            RiderPhone = riderPhone,
            TrackingUrl = trackingUrl,
            ProviderName = providerName,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static TaskStatusResult Failure(string errorMessage, string providerName) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage, ProviderName = providerName };
}