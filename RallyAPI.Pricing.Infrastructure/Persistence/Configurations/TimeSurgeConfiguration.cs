// RallyAPI.Pricing.Infrastructure/Persistence/Configurations/TimeSurgeConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RallyAPI.Pricing.Domain.Entities;

namespace RallyAPI.Pricing.Infrastructure.Persistence.Configurations;

public class TimeSurgeConfiguration : IEntityTypeConfiguration<TimeSurge>
{
    public void Configure(EntityTypeBuilder<TimeSurge> builder)
    {
        builder.ToTable("time_surges");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DayOfWeek).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.EndTime).IsRequired();
        builder.Property(x => x.SurgeAmount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}