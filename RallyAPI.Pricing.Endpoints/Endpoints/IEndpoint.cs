// RallyAPI.Pricing.Endpoints/IEndpoint.cs
using Microsoft.AspNetCore.Routing;

namespace RallyAPI.Pricing.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}