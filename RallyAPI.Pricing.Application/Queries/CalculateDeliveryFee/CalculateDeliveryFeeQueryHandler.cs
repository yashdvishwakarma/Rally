// RallyAPI.Pricing.Application/Queries/CalculateDeliveryFee/CalculateDeliveryFeeQueryHandler.cs
using MediatR;
using RallyAPI.Pricing.Application.Abstractions;
using RallyAPI.Pricing.Application.DTOs;
using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.ValueObjects;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Pricing.Application.Queries.CalculateDeliveryFee;

public class CalculateDeliveryFeeQueryHandler
    : IRequestHandler<CalculateDeliveryFeeQuery, Result<DeliveryFeeResponse>>
{
    private readonly IPricingEngine _pricingEngine;
    private readonly IWeatherProvider _weatherProvider;
    private readonly IDemandTracker _demandTracker;

    public CalculateDeliveryFeeQueryHandler(
        IPricingEngine pricingEngine,
        IWeatherProvider weatherProvider,
        IDemandTracker demandTracker)
    {
        _pricingEngine = pricingEngine;
        _weatherProvider = weatherProvider;
        _demandTracker = demandTracker;
    }

    public async Task<Result<DeliveryFeeResponse>> Handle(
        CalculateDeliveryFeeQuery request,
        CancellationToken cancellationToken)
    {
        // Get external data
        var weather = await _weatherProvider.GetCurrentWeatherAsync(
            request.CustomerLatitude,
            request.CustomerLongitude,
            cancellationToken);

        var ordersPerHour = await _demandTracker.GetCurrentOrdersPerHourAsync(
            request.RestaurantId,
            cancellationToken);

        // Build context
        var context = new PricingContext(
            RestaurantLatitude: request.RestaurantLatitude,
            RestaurantLongitude: request.RestaurantLongitude,
            CustomerLatitude: request.CustomerLatitude,
            CustomerLongitude: request.CustomerLongitude,
            OrderTime: DateTime.UtcNow,
            DayOfWeek: DateTime.UtcNow.DayOfWeek,
            OrderSubtotal: request.OrderSubtotal,
            ItemCount: request.ItemCount,
            RestaurantId: request.RestaurantId,
            CustomerId: request.CustomerId,
            Weather: weather,
            CurrentOrdersPerHour: ordersPerHour,
            PromoCode: request.PromoCode);

        // Calculate
        var result = await _pricingEngine.CalculateDeliveryFeeAsync(context, cancellationToken);

        // Map response
        var response = new DeliveryFeeResponse(
            result.BaseFee,
            result.FinalFee,
            result.SurgeMultiplier,
            result.PrimarySurgeReason,
            context.DistanceKm,
            result.Breakdown.Select(b => new FeeBreakdownItem(b.RuleName, b.Description, b.Amount)).ToList());

        return Result.Success(response);
    }
}