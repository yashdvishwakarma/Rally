// RallyAPI.Pricing.Infrastructure/Persistence/Configurations/DemandSurgeConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RallyAPI.Pricing.Domain.Entities;

namespace RallyAPI.Pricing.Infrastructure.Persistence.Configurations;

public class DemandSurgeConfiguration : IEntityTypeConfiguration<DemandSurge>
{
    public void Configure(EntityTypeBuilder<DemandSurge> builder)
    {
        builder.ToTable("demand_surges");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MinOrdersPerHour).IsRequired();
        builder.Property(x => x.MaxOrdersPerHour);
        builder.Property(x => x.Multiplier).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}