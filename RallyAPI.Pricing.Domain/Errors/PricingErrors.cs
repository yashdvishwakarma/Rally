using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Pricing.Domain.Errors;

public static class PricingErrors
{
    public static readonly Error ConfigNotFound = Error.Create(
        "Pricing.ConfigNotFound",
        "Pricing configuration not found");

    public static readonly Error InvalidDistance = Error.Create(
        "Pricing.InvalidDistance",
        "Invalid distance calculation");

    public static readonly Error NoActiveRules = Error.Create(
        "Pricing.NoActiveRules",
        "No active pricing rules found");

    public static readonly Error CalculationFailed = Error.Create(
        "Pricing.CalculationFailed",
        "Failed to calculate delivery fee");
}