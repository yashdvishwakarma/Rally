// RallyAPI.Pricing.Infrastructure/Persistence/Configurations/BaseFeeConfigConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RallyAPI.Pricing.Domain.Entities;

namespace RallyAPI.Pricing.Infrastructure.Persistence.Configurations;

public class BaseFeeConfigConfiguration : IEntityTypeConfiguration<BaseFeeConfig>
{
    public void Configure(EntityTypeBuilder<BaseFeeConfig> builder)
    {
        builder.ToTable("base_fee_configs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.MinimumFee).HasPrecision(10, 2);
        builder.Property(x => x.MaximumFee).HasPrecision(10, 2);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}