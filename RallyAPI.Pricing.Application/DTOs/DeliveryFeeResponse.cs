// RallyAPI.Pricing.Application/DTOs/DeliveryFeeResponse.cs
namespace RallyAPI.Pricing.Application.DTOs;

public record DeliveryFeeResponse(
    decimal BaseFee,
    decimal FinalFee,
    decimal SurgeMultiplier,
    string? SurgeReason,
    double DistanceKm,
    List<FeeBreakdownItem> Breakdown);

public record FeeBreakdownItem(
    string Name,
    string Description,
    decimal Amount);