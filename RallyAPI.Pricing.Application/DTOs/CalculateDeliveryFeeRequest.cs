// RallyAPI.Pricing.Application/DTOs/CalculateDeliveryFeeRequest.cs
namespace RallyAPI.Pricing.Application.DTOs;

public record CalculateDeliveryFeeRequest(
    double RestaurantLatitude,
    double RestaurantLongitude,
    double CustomerLatitude,
    double CustomerLongitude,
    decimal OrderSubtotal,
    Guid RestaurantId,
    Guid? CustomerId = null,
    string? PromoCode = null);