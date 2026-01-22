using RallyAPI.Orders.Domain.Entities;
using RallyAPI.Orders.Domain.ValueObjects;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Domain.Abstractions;

/// <summary>
/// Service interface for calculating order pricing.
/// Abstracted for flexibility - can be swapped for different pricing strategies.
/// </summary>
public interface IOrderPricingService
{
    /// <summary>
    /// Calculates complete pricing for an order.
    /// </summary>
    Task<Result<OrderPricing>> CalculatePricingAsync(
        IReadOnlyCollection<OrderItem> items,
        decimal deliveryFee,
        string? discountCode = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates subtotal from items.
    /// </summary>
    Money CalculateSubTotal(IReadOnlyCollection<OrderItem> items);

    /// <summary>
    /// Calculates tax based on subtotal and location.
    /// </summary>
    Money CalculateTax(Money subTotal, string? city = null, string? pincode = null);

    /// <summary>
    /// Validates and applies discount code.
    /// </summary>
    Task<Result<Money>> ValidateAndCalculateDiscountAsync(
        string discountCode,
        Money subTotal,
        Guid customerId,
        Guid restaurantId,
        CancellationToken cancellationToken = default);
}