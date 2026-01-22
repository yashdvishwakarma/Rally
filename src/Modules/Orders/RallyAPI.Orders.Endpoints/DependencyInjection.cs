using Microsoft.Extensions.DependencyInjection;
using RallyAPI.Orders.Application;
using RallyAPI.Orders.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace RallyAPI.Orders.Endpoints;

public static class DependencyInjection
{
    /// <summary>
    /// Adds all Orders module services.
    /// </summary>
    public static IServiceCollection AddOrdersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Application layer (MediatR, Validators, Behaviors)
        services.AddOrdersApplication();

        // Add Infrastructure layer (DbContext, Repositories, Services)
        services.AddOrdersInfrastructure(configuration);

        return services;
    }
}