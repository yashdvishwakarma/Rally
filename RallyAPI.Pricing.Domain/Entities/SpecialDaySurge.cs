// RallyAPI.Pricing.Domain/Entities/SpecialDaySurge.cs
using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Pricing.Domain.Entities;

public class SpecialDaySurge : BaseEntity
{
    public DateOnly Date { get; private set; }
    public decimal SurgeAmount { get; private set; }
    public decimal? Multiplier { get; private set; }
    public string Reason { get; private set; } = default!;  // "Diwali", "New Year", etc.
    public bool IsActive { get; private set; }

    private SpecialDaySurge() { }

    public static SpecialDaySurge Create(
        DateOnly date,
        decimal surgeAmount,
        decimal? multiplier,
        string reason)
    {
        return new SpecialDaySurge
        {
            Id = Guid.NewGuid(),
            Date = date,
            SurgeAmount = surgeAmount,
            Multiplier = multiplier,
            Reason = reason,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}