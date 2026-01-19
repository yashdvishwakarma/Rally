namespace RallyAPI.SharedKernel.Abstractions.Delivery;

/// <summary>
/// Result of a delivery quote request.
/// Uses factory methods to ensure valid state.
/// </summary>
public sealed record DeliveryQuoteResult
{
    public bool IsSuccess { get; private init; }
    public string? QuoteId { get; private init; }
    public decimal? Price { get; private init; }
    public int? EstimatedMinutes { get; private init; }
    public string? ErrorMessage { get; private init; }
    public string? ProviderName { get; private init; }

    /// <summary>
    /// Timestamp when this quote was retrieved.
    /// Useful for cache invalidation and quote expiry.
    /// </summary>
    public DateTimeOffset RetrievedAt { get; private init; }

    private DeliveryQuoteResult() { }

    /// <summary>
    /// Creates a successful quote result.
    /// </summary>
    public static DeliveryQuoteResult Success(
        string quoteId,
        decimal price,
        int estimatedMinutes,
        string? providerName = null)
    {
        return new DeliveryQuoteResult
        {
            IsSuccess = true,
            QuoteId = quoteId,
            Price = price,
            EstimatedMinutes = estimatedMinutes,
            ProviderName = providerName,
            RetrievedAt = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    /// Creates a failed quote result with error details.
    /// </summary>
    public static DeliveryQuoteResult Failure(string errorMessage, string? providerName = null)
    {
        return new DeliveryQuoteResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ProviderName = providerName,
            RetrievedAt = DateTimeOffset.UtcNow
        };
    }
}