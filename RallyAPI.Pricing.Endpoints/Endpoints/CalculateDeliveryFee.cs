using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RallyAPI.Pricing.Application.DTOs;
using RallyAPI.Pricing.Application.Queries.CalculateDeliveryFee;

namespace RallyAPI.Pricing.Endpoints.Endpoints;

public class CalculateDeliveryFee : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/pricing/delivery-fee", HandleAsync)
            .WithTags("Pricing")
            .WithSummary("Calculate delivery fee")
            .WithDescription("Returns delivery fee with breakdown of all applied rules")
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleAsync(
        CalculateDeliveryFeeRequest request,
        ISender sender,
        CancellationToken ct)
    {
        var query = new CalculateDeliveryFeeQuery(
            request.RestaurantLatitude,
            request.RestaurantLongitude,
            request.CustomerLatitude,
            request.CustomerLongitude,
            request.OrderSubtotal,
            ItemCount: 1, // Can be added to request
            request.RestaurantId,
            request.CustomerId,
            request.PromoCode);

        var result = await sender.Send(query, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }
}