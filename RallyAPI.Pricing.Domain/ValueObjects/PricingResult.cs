// RallyAPI.Pricing.Domain/ValueObjects/PricingResult.cs
namespace RallyAPI.Pricing.Domain.ValueObjects;

public record PricingResult(
    string QuoteId,                              // ← ADD
    DateTime ExpiresAt,                          // ← ADD
    decimal BaseFee,
    decimal FinalFee,
    decimal SurgeMultiplier,
    string? PrimarySurgeReason,
    DeliveryQuote? ThirdPartyQuote,             // ← ADD (optional 3PL quote)
    IReadOnlyList<AppliedModification> Breakdown)
{
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    public static PricingResult Empty => new(
        string.Empty,
        DateTime.UtcNow,
        0, 0, 1, null, null,
        new List<AppliedModification>());
}

public record AppliedModification(
    string RuleName,
    string Description,
    decimal Amount);