// RallyAPI.Pricing.Infrastructure/Persistence/Configurations/WeatherSurgeConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RallyAPI.Pricing.Domain.Entities;

namespace RallyAPI.Pricing.Infrastructure.Persistence.Configurations;

public class WeatherSurgeConfiguration : IEntityTypeConfiguration<WeatherSurge>
{
    public void Configure(EntityTypeBuilder<WeatherSurge> builder)
    {
        builder.ToTable("weather_surges");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Condition).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(x => x.SurgeAmount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Multiplier).HasPrecision(5, 2);
        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.Condition);
    }
}