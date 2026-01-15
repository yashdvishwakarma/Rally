using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Domain.Abstractions;

public interface IPricingRule
{
    /// <summary>
    /// Lower number = runs first
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Rule identifier for logging/debugging
    /// </summary>
    string RuleName { get; }

    /// <summary>
    /// Is this rule enabled?
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Should this rule apply given the context?
    /// </summary>
    Task<bool> AppliesAsync(PricingContext context, CancellationToken ct = default);

    /// <summary>
    /// Calculate the modification
    /// </summary>
    Task<PriceModification?> CalculateAsync(PricingContext context, CancellationToken ct = default);
}