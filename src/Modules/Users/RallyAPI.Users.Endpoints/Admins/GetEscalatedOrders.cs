using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RallyAPI.SharedKernel.Abstractions.Orders;

namespace RallyAPI.Users.Endpoints.Admins;

public class GetEscalatedOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admins/orders/escalated", HandleAsync)
            .WithName("GetEscalatedOrders")
            .WithTags("Admins")
            .RequireAuthorization("Admin");
    }

    private static async Task<IResult> HandleAsync(
        int page,
        int pageSize,
        IEscalatedOrderQueryService escalatedOrderService,
        CancellationToken cancellationToken)
    {
        var result = await escalatedOrderService.GetEscalatedOrdersAsync(
            page: page > 0 ? page : 1,
            pageSize: pageSize is > 0 and <= 100 ? pageSize : 20,
            cancellationToken);

        return Results.Ok(result);
    }
}
