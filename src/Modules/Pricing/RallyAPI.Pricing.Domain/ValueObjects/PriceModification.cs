using RallyAPI.Pricing.Domain.Enums;

namespace RallyAPI.Pricing.Domain.ValueObjects;

public record PriceModification(
    string RuleName,
    string Description,
    decimal Amount,
    ModificationType Type,
    int Priority)
{
    public decimal Apply(decimal currentAmount)
    {
        return Type switch
        {
            ModificationType.Flat => Amount,
            ModificationType.Percentage => currentAmount * (Amount / 100),
            ModificationType.Multiplier => currentAmount * (Amount - 1), // 1.5x means add 0.5x
            _ => 0
        };
    }
}