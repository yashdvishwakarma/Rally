// RallyAPI.Pricing.Domain/Abstractions/IDeliveryQuoteProvider.cs
namespace RallyAPI.Pricing.Domain.Abstractions;

public interface IDeliveryQuoteProvider
{
    string ProviderName { get; }

    Task<DeliveryQuoteResult> GetQuoteAsync(
        DeliveryQuoteRequest request,
        CancellationToken ct = default);
}

public record DeliveryQuoteRequest(
    double PickupLatitude,
    double PickupLongitude,
    string PickupPincode,
    double DropLatitude,
    double DropLongitude,
    string DropPincode,
    string City,
    decimal OrderAmount,
    decimal? OrderWeight = null);

public record DeliveryQuoteResult(
    bool IsSuccess,
    string? QuoteId,
    decimal? Price,
    int? EstimatedMinutes,
    string? ErrorMessage)
{
    public static DeliveryQuoteResult Success(
        string quoteId,
        decimal price,
        int estimatedMinutes)
        => new(true, quoteId, price, estimatedMinutes, null);

    public static DeliveryQuoteResult Failure(string error)
        => new(false, null, null, null, error);
}