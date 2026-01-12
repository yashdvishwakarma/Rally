using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RallyAPI.Catalog.Application.Abstractions;
using RallyAPI.Catalog.Infrastructure.Persistence;
using RallyAPI.Catalog.Infrastructure.Persistence.Repositories;

namespace RallyAPI.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Database"),
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "catalog")));

        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}