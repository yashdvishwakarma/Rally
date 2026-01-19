// RallyAPI.Pricing.Application/Queries/CalculateDeliveryFee/CalculateDeliveryFeeQuery.cs
using MediatR;
using RallyAPI.Pricing.Application.DTOs;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Pricing.Application.Queries.CalculateDeliveryFee;

public record CalculateDeliveryFeeQuery(
    double RestaurantLatitude,
    double RestaurantLongitude,
    string RestaurantPincode,
    double CustomerLatitude,
    double CustomerLongitude,
    string CustomerPincode,
    string City,
    decimal OrderSubtotal,
    decimal? OrderWeight,
    int ItemCount,
    Guid RestaurantId,
    Guid? CustomerId = null,
    string? PromoCode = null) : IRequest<Result<DeliveryFeeResponse>>;