// RallyAPI.Pricing.Domain/ValueObjects/DeliveryQuote.cs
namespace RallyAPI.Pricing.Domain.ValueObjects;

public record DeliveryQuote(
    string QuoteId,
    string ProviderName,
    decimal Price,
    int EstimatedMinutes,
    DateTime ExpiresAt)
{
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    public static DeliveryQuote CreateWithExpiry(
        string quoteId,
        string providerName,
        decimal price,
        int estimatedMinutes,
        int expiryMinutes = 10)
    {
        return new DeliveryQuote(
            quoteId,
            providerName,
            price,
            estimatedMinutes,
            DateTime.UtcNow.AddMinutes(expiryMinutes));
    }
}