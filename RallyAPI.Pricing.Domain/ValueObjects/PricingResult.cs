namespace RallyAPI.Pricing.Domain.ValueObjects;

public record PricingResult(
    decimal BaseFee,
    decimal FinalFee,
    decimal SurgeMultiplier,
    string? PrimarySurgeReason,
    IReadOnlyList<AppliedModification> Breakdown)
{
    public static PricingResult Empty => new(0, 0, 1, null, new List<AppliedModification>());
}

public record AppliedModification(
    string RuleName,
    string Description,
    decimal Amount);