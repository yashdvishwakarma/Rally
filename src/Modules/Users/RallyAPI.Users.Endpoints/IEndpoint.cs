using Microsoft.AspNetCore.Routing;

namespace RallyAPI.Users.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}