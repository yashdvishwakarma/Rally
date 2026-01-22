// RallyAPI.Pricing.Infrastructure/Persistence/Configurations/SpecialDaySurgeConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RallyAPI.Pricing.Domain.Entities;

namespace RallyAPI.Pricing.Infrastructure.Persistence.Configurations;

public class SpecialDaySurgeConfiguration : IEntityTypeConfiguration<SpecialDaySurge>
{
    public void Configure(EntityTypeBuilder<SpecialDaySurge> builder)
    {
        builder.ToTable("special_day_surges");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.SurgeAmount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Multiplier).HasPrecision(5, 2);
        builder.Property(x => x.Reason).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.Date);
    }
}