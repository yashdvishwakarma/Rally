using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RallyAPI.Users.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersEndpoints(this IServiceCollection services)
    {
        // Nothing needed here - MediatR registered in Host
        return services;
    }

    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
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