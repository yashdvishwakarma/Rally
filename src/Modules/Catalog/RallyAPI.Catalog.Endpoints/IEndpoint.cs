using Microsoft.AspNetCore.Routing;

namespace RallyAPI.Catalog.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}