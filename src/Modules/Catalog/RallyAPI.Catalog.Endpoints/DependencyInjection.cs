using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RallyAPI.Catalog.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogEndpoints(this IServiceCollection services)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointTypes = typeof(DependencyInjection).Assembly
            .GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var endpointType in endpointTypes)
        {
            var endpoint = (IEndpoint)Activator.CreateInstance(endpointType)!;
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}