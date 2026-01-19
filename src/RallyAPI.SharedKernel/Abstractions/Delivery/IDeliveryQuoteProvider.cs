namespace RallyAPI.SharedKernel.Abstractions.Delivery;

/// <summary>
/// Abstraction for third-party delivery quote providers.
/// Implement this interface to integrate with delivery partners.
/// </summary>
public interface IDeliveryQuoteProvider
{
    /// <summary>
    /// Provider identifier (e.g., "ProRouting", "Dunzo", "Shadowfax")
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Gets a delivery quote from the provider.
    /// </summary>
    Task<DeliveryQuoteResult> GetQuoteAsync(
        DeliveryQuoteRequest request,
        CancellationToken cancellationToken = default);
}