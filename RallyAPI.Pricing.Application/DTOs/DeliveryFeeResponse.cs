// RallyAPI.Pricing.Application/DTOs/DeliveryFeeResponse.cs
namespace RallyAPI.Pricing.Application.DTOs;

public record DeliveryFeeResponse(
    string QuoteId,                    // ← ADD
    DateTime ExpiresAt,                // ← ADD
    decimal BaseFee,
    decimal FinalFee,
    decimal SurgeMultiplier,
    string? SurgeReason,
    double DistanceKm,
    ThirdPartyQuoteResponse? ThirdPartyQuote,  // ← ADD
    List<FeeBreakdownItem> Breakdown);

public record ThirdPartyQuoteResponse(
    string QuoteId,
    string ProviderName,
    decimal Price,
    int EstimatedMinutes,
    DateTime ExpiresAt);

public record FeeBreakdownItem(
    string Name,
    string Description,
    decimal Amount);