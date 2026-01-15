// RallyAPI.Pricing.Endpoints/DependencyInjection.cs
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RallyAPI.Pricing.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddPricingEndpoints(this IServiceCollection services)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapPricingEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = typeof(DependencyInjection).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IEndpoint)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}