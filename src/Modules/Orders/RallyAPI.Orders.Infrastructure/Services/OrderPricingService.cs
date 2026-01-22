using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RallyAPI.Orders.Domain.Abstractions;
using RallyAPI.Orders.Domain.Entities;
using RallyAPI.Orders.Domain.ValueObjects;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Infrastructure.Services;

/// <summary>
/// Implementation of order pricing service.
/// Configurable and extensible for different pricing strategies.
/// </summary>
public sealed class OrderPricingService : IOrderPricingService
{
    private readonly ILogger<OrderPricingService> _logger;
    private readonly PricingOptions _options;

    public OrderPricingService(
        ILogger<OrderPricingService> logger,
        IOptions<PricingOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public Task<Result<OrderPricing>> CalculatePricingAsync(
        IReadOnlyCollection<OrderItem> items,
        decimal deliveryFee,
        string? discountCode = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var currency = _options.DefaultCurrency;
            var subTotal = CalculateSubTotal(items);
            var tax = CalculateTax(subTotal);
            var discount = Money.Zero(currency);

            // TODO: Apply discount code if provided
            // For now, just log it
            if (!string.IsNullOrWhiteSpace(discountCode))
            {
                _logger.LogInformation("Discount code {Code} provided but not implemented yet", discountCode);
            }

            var pricing = OrderPricing.Create(
                subTotal,
                Money.FromDecimal(deliveryFee, currency),
                tax,
                discount);

            _logger.LogDebug(
                "Pricing calculated: SubTotal={SubTotal}, Tax={Tax}, Delivery={Delivery}, Total={Total}",
                subTotal.Amount,
                tax.Amount,
                deliveryFee,
                pricing.Total.Amount);

            return Task.FromResult(Result.Success(pricing));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating order pricing");
            return Task.FromResult(Result.Failure<OrderPricing>(
                Error.Create("Pricing.Error", $"Failed to calculate pricing: {ex.Message}")));
        }
    }

    public Money CalculateSubTotal(IReadOnlyCollection<OrderItem> items)
    {
        if (items == null || items.Count == 0)
        {
            return Money.Zero(_options.DefaultCurrency);
        }

        var total = items.Sum(i => i.TotalPrice.Amount);
        return Money.FromDecimal(total, _options.DefaultCurrency);
    }

    public Money CalculateTax(Money subTotal, string? city = null, string? pincode = null)
    {
        // MVP: Use default tax rate
        // TODO: Implement location-based tax calculation
        var taxRate = _options.DefaultTaxRate;

        // Could vary by city/state in future
        if (!string.IsNullOrWhiteSpace(city))
        {
            // Example: Different tax for different cities
            // taxRate = GetTaxRateForCity(city);
        }

        var taxAmount = subTotal.Amount * taxRate;
        return Money.FromDecimal(taxAmount, subTotal.Currency);
    }

    public Task<Result<Money>> ValidateAndCalculateDiscountAsync(
        string discountCode,
        Money subTotal,
        Guid customerId,
        Guid restaurantId,
        CancellationToken cancellationToken = default)
    {
        // MVP: No discount implementation
        // TODO: Implement discount code validation and calculation
        _logger.LogInformation(
            "Discount code validation requested: {Code} for Customer {CustomerId}",
            discountCode,
            customerId);

        // Return zero discount for now
        return Task.FromResult(Result.Success(Money.Zero(subTotal.Currency)));
    }
}

/// <summary>
/// Configuration options for pricing.
/// </summary>
public sealed class PricingOptions
{
    public const string SectionName = "Pricing";

    public string DefaultCurrency { get; set; } = "INR";
    public decimal DefaultTaxRate { get; set; } = 0.05m; // 5%
    public decimal MinimumOrderAmount { get; set; } = 50m;
    public decimal FreeDeliveryThreshold { get; set; } = 500m;
    public decimal BaseDeliveryFee { get; set; } = 30m;
    public decimal PerKmDeliveryCharge { get; set; } = 10m;
    public decimal MaxDeliveryFee { get; set; } = 150m;
}