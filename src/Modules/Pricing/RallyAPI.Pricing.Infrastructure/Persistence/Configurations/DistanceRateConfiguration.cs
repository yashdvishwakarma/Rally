// RallyAPI.Pricing.Infrastructure/Persistence/Configurations/DistanceRateConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RallyAPI.Pricing.Domain.Entities;

namespace RallyAPI.Pricing.Infrastructure.Persistence.Configurations;

public class DistanceRateConfiguration : IEntityTypeConfiguration<DistanceRate>
{
    public void Configure(EntityTypeBuilder<DistanceRate> builder)
    {
        builder.ToTable("distance_rates");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MinDistanceKm).IsRequired();
        builder.Property(x => x.MaxDistanceKm).IsRequired();
        builder.Property(x => x.Rate).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => new { x.MinDistanceKm, x.MaxDistanceKm });
    }
}