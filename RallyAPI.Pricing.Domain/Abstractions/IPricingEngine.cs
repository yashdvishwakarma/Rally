// RallyAPI.Pricing.Domain/Abstractions/IPricingEngine.cs
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Domain.Abstractions;

public interface IPricingEngine
{
    Task<PricingResult> CalculateDeliveryFeeAsync(
        PricingContext context,
        CancellationToken ct = default);
}