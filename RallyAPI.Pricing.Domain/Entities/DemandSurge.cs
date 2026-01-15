// RallyAPI.Pricing.Domain/Entities/DemandSurge.cs
using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Pricing.Domain.Entities;

public class DemandSurge : BaseEntity
{
    public int MinOrdersPerHour { get; private set; }
    public int? MaxOrdersPerHour { get; private set; }
    public decimal Multiplier { get; private set; }
    public string Description { get; private set; } = default!;
    public bool IsActive { get; private set; }

    private DemandSurge() { }

    public static DemandSurge Create(
        int minOrders,
        int? maxOrders,
        decimal multiplier,
        string description)
    {
        return new DemandSurge
        {
            Id = Guid.NewGuid(),
            MinOrdersPerHour = minOrders,
            MaxOrdersPerHour = maxOrders,
            Multiplier = multiplier,
            Description = description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public bool Applies(int currentOrdersPerHour)
    {
        if (!IsActive) return false;

        var meetsMin = currentOrdersPerHour >= MinOrdersPerHour;
        var meetsMax = MaxOrdersPerHour == null || currentOrdersPerHour < MaxOrdersPerHour;

        return meetsMin && meetsMax;
    }
}