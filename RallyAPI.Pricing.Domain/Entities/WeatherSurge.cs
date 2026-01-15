using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Pricing.Domain.Entities;

public class WeatherSurge : BaseEntity
{
    public WeatherCondition Condition { get; private set; }
    public decimal SurgeAmount { get; private set; }
    public decimal? Multiplier { get; private set; }  // Optional: use multiplier instead
    public string Description { get; private set; } = default!;
    public bool IsActive { get; private set; }

    private WeatherSurge() { }

    public static WeatherSurge Create(
        WeatherCondition condition,
        decimal surgeAmount,
        decimal? multiplier,
        string description)
    {
        return new WeatherSurge
        {
            Id = Guid.NewGuid(),
            Condition = condition,
            SurgeAmount = surgeAmount,
            Multiplier = multiplier,
            Description = description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}