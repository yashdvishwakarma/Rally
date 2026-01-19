// RallyAPI.Pricing.Endpoints/Endpoints/CalculateDeliveryFee.cs
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
            .WithSummary("Calculate delivery fee with quote")
            .WithDescription("Returns delivery fee with quote ID valid for 10 minutes")
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
            request.RestaurantPincode,
            request.CustomerLatitude,
            request.CustomerLongitude,
            request.CustomerPincode,
            request.City,
            request.OrderSubtotal,
            request.OrderWeight,
            ItemCount: 1,
            request.RestaurantId,
            request.CustomerId,
            request.PromoCode);

        var result = await sender.Send(query, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }
}