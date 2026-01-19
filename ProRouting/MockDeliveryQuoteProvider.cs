using Microsoft.Extensions.Logging;
using RallyAPI.SharedKernel.Abstractions.Delivery;

namespace RallyAPI.Integrations.ProRouting;

/// <summary>
/// Mock implementation of IDeliveryQuoteProvider for testing.
/// Returns predictable responses without calling external APIs.
/// </summary>
public sealed class MockDeliveryQuoteProvider : IDeliveryQuoteProvider
{
    private readonly ILogger<MockDeliveryQuoteProvider> _logger;
    private readonly MockQuoteOptions _mockOptions;

    public string ProviderName => "Mock";

    public MockDeliveryQuoteProvider(
        ILogger<MockDeliveryQuoteProvider> logger,
        MockQuoteOptions? mockOptions = null)
    {
        _logger = logger;
        _mockOptions = mockOptions ?? new MockQuoteOptions();
    }

    public Task<DeliveryQuoteResult> GetQuoteAsync(
        DeliveryQuoteRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug(
            "Mock quote requested. City: {City}, Amount: {Amount}",
            request.City,
            request.OrderAmount);

        if (_mockOptions.ShouldFail)
        {
            return Task.FromResult(
                DeliveryQuoteResult.Failure(_mockOptions.FailureMessage, ProviderName));
        }

        // Generate a realistic-looking quote ID
        var quoteId = $"mock_{Guid.NewGuid():N}"[..24];

        // Calculate a mock price based on order amount (for more realistic testing)
        var basePrice = _mockOptions.BasePrice;
        var priceVariation = (request.OrderAmount * 0.1m); // 10% of order amount
        var finalPrice = Math.Max(basePrice, basePrice + priceVariation * (Random.Shared.Next(-1, 2)));

        var result = DeliveryQuoteResult.Success(
            quoteId: quoteId,
            price: Math.Round(finalPrice, 2),
            estimatedMinutes: _mockOptions.EstimatedMinutes,
            providerName: ProviderName);

        _logger.LogInformation(
            "Mock quote generated. QuoteId: {QuoteId}, Price: {Price}",
            quoteId,
            finalPrice);

        return Task.FromResult(result);
    }
}

/// <summary>
/// Configuration options for mock quote behavior.
/// </summary>
public sealed class MockQuoteOptions
{
    /// <summary>
    /// If true, the mock provider will return failures.
    /// </summary>
    public bool ShouldFail { get; set; } = false;

    /// <summary>
    /// Error message when ShouldFail is true.
    /// </summary>
    public string FailureMessage { get; set; } = "Mock provider configured to fail";

    /// <summary>
    /// Base price for mock quotes.
    /// </summary>
    public decimal BasePrice { get; set; } = 50m;

    /// <summary>
    /// Estimated delivery time in minutes.
    /// </summary>
    public int EstimatedMinutes { get; set; } = 30;
}