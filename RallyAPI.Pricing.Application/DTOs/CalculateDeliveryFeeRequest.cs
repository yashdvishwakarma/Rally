// RallyAPI.Pricing.Application/DTOs/CalculateDeliveryFeeRequest.cs
namespace RallyAPI.Pricing.Application.DTOs;

public record CalculateDeliveryFeeRequest(
    double RestaurantLatitude,
    double RestaurantLongitude,
    string RestaurantPincode,          // ← ADD
    double CustomerLatitude,
    double CustomerLongitude,
    string CustomerPincode,            // ← ADD
    string City,                       // ← ADD
    decimal OrderSubtotal,
    decimal? OrderWeight,              // ← ADD
    Guid RestaurantId,
    Guid? CustomerId = null,
    string? PromoCode = null);