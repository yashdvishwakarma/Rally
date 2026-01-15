using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Pricing.Domain.Entities;

public class DistanceRate : BaseEntity
{
    public double MinDistanceKm { get; private set; }
    public double MaxDistanceKm { get; private set; }
    public decimal Rate { get; private set; }
    public bool IsActive { get; private set; }

    private DistanceRate() { }

    public static DistanceRate Create(
        double minKm,
        double maxKm,
        decimal rate)
    {
        return new DistanceRate
        {
            Id = Guid.NewGuid(),
            MinDistanceKm = minKm,
            MaxDistanceKm = maxKm,
            Rate = rate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public bool IsInRange(double distanceKm)
    {
        return distanceKm >= MinDistanceKm && distanceKm < MaxDistanceKm;
    }

    public void UpdateRate(decimal newRate)
    {
        Rate = newRate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}