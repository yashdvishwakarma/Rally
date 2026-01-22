// RallyAPI.Pricing.Domain/Entities/TimeSurge.cs
using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Pricing.Domain.Entities;

public class TimeSurge : BaseEntity
{
    public DayOfWeek? DayOfWeek { get; private set; }  // Null = all days
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public decimal SurgeAmount { get; private set; }
    public string Description { get; private set; } = default!;
    public bool IsActive { get; private set; }

    private TimeSurge() { }

    public static TimeSurge Create(
        DayOfWeek? dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        decimal surgeAmount,
        string description)
    {
        return new TimeSurge
        {
            Id = Guid.NewGuid(),
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            SurgeAmount = surgeAmount,
            Description = description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public bool Applies(DateTime orderTime)
    {
        if (!IsActive) return false;

        var time = TimeOnly.FromDateTime(orderTime);
        var dayMatches = DayOfWeek == null || DayOfWeek == orderTime.DayOfWeek;
        var timeMatches = time >= StartTime && time <= EndTime;

        return dayMatches && timeMatches;
    }
}