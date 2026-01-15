// RallyAPI.Pricing.Infrastructure/Persistence/PricingDbContext.cs
using Microsoft.EntityFrameworkCore;
using RallyAPI.Pricing.Application.Abstractions;
using RallyAPI.Pricing.Domain.Entities;

namespace RallyAPI.Pricing.Infrastructure.Persistence;

public class PricingDbContext : DbContext, IUnitOfWork
{
    public DbSet<BaseFeeConfig> BaseFeeConfigs => Set<BaseFeeConfig>();
    public DbSet<DistanceRate> DistanceRates => Set<DistanceRate>();
    public DbSet<TimeSurge> TimeSurges => Set<TimeSurge>();
    public DbSet<WeatherSurge> WeatherSurges => Set<WeatherSurge>();
    public DbSet<DemandSurge> DemandSurges => Set<DemandSurge>();
    public DbSet<SpecialDaySurge> SpecialDaySurges => Set<SpecialDaySurge>();

    public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("pricing");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PricingDbContext).Assembly);
    }
}