namespace RallyAPI.SharedKernel.Abstractions.Delivery;

/// <summary>
/// Represents a single quote from a third-party logistics provider.
/// </summary>
public sealed record ThirdPartyLspQuote
{
    /// <summary>
    /// LSP identifier (e.g., "preprod.porter-lsp.mp2.in").
    /// </summary>
    public required string LspId { get; init; }

    /// <summary>
    /// Item/service ID from the LSP.
    /// </summary>
    public required string ItemId { get; init; }

    /// <summary>
    /// Display name (e.g., "Porter", "Zypp").
    /// </summary>
    public required string LogisticsSeller { get; init; }

    /// <summary>
    /// Forward delivery price.
    /// </summary>
    public required decimal PriceForward { get; init; }

    /// <summary>
    /// Return to origin price (if delivery fails).
    /// </summary>
    public required decimal PriceRto { get; init; }

    /// <summary>
    /// Total SLA in minutes (full delivery time).
    /// </summary>
    public required int SlaMins { get; init; }

    /// <summary>
    /// Pickup ETA in minutes (rider arrival at restaurant).
    /// </summary>
    public required int PickupEtaMins { get; init; }
}

/// <summary>
/// Response from getting quotes from a third-party provider.
/// Contains multiple LSP options.
/// </summary>
public sealed record ThirdPartyQuotesResult
{
    private ThirdPartyQuotesResult() { }

    public bool IsSuccess { get; private init; }

    /// <summary>
    /// Quote ID to use when booking (from provider).
    /// </summary>
    public string? QuoteId { get; private init; }

    /// <summary>
    /// When these quotes expire.
    /// </summary>
    public DateTime? ValidUntil { get; private init; }

    /// <summary>
    /// Available LSP quotes.
    /// </summary>
    public IReadOnlyList<ThirdPartyLspQuote> Quotes { get; private init; } = [];

    /// <summary>
    /// Error message if failed.
    /// </summary>
    public string? ErrorMessage { get; private init; }

    /// <summary>
    /// Provider name.
    /// </summary>
    public string? ProviderName { get; private init; }

    /// <summary>
    /// When the quotes were retrieved.
    /// </summary>
    public DateTime RetrievedAt { get; private init; }

    public static ThirdPartyQuotesResult Success(
        string quoteId,
        DateTime validUntil,
        IReadOnlyList<ThirdPartyLspQuote> quotes,
        string providerName)
    {
        return new ThirdPartyQuotesResult
        {
            IsSuccess = true,
            QuoteId = quoteId,
            ValidUntil = validUntil,
            Quotes = quotes,
            ProviderName = providerName,
            RetrievedAt = DateTime.UtcNow
        };
    }

    public static ThirdPartyQuotesResult Failure(string errorMessage, string providerName)
    {
        return new ThirdPartyQuotesResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ProviderName = providerName,
            Quotes = [],
            RetrievedAt = DateTime.UtcNow
        };
    }
}