// RallyAPI.Pricing.Application/Rules/ThirdPartyDeliveryRule.cs
using RallyAPI.Pricing.Domain.Abstractions;
using RallyAPI.Pricing.Domain.Enums;
using RallyAPI.Pricing.Domain.ValueObjects;

namespace RallyAPI.Pricing.Application.Rules;

public class ThirdPartyDeliveryRule : IPricingRule
{
    private readonly IDeliveryQuoteProvider? _quoteProvider;

    public ThirdPartyDeliveryRule(IDeliveryQuoteProvider? quoteProvider = null)
    {
        _quoteProvider = quoteProvider;
    }

    public int Priority => 100; // Runs last
    public string RuleName => "ThirdPartyDelivery";
    public bool IsEnabled => _quoteProvider != null;

    public Task<bool> AppliesAsync(PricingContext context, CancellationToken ct = default)
    {
        // Only apply if we have a provider configured
        return Task.FromResult(_quoteProvider != null);
    }

    public async Task<PriceModification?> CalculateAsync(
        PricingContext context,
        CancellationToken ct = default)
    {
        if (_quoteProvider == null) return null;

        var request = new DeliveryQuoteRequest(
            PickupLatitude: context.RestaurantLatitude,
            PickupLongitude: context.RestaurantLongitude,
            PickupPincode: context.PickupPincode ?? "",
            DropLatitude: context.CustomerLatitude,
            DropLongitude: context.CustomerLongitude,
            DropPincode: context.DropPincode ?? "",
            City: context.City ?? "",
            OrderAmount: context.OrderSubtotal,
            OrderWeight: context.OrderWeight);

        var result = await _quoteProvider.GetQuoteAsync(request, ct);

        if (!result.IsSuccess) return null;

        // Store quote info in context for later use
        context.SetThirdPartyQuote(
            result.QuoteId!,
            _quoteProvider.ProviderName,
            result.Price!.Value,
            result.EstimatedMinutes!.Value);

        return new PriceModification(
            RuleName,
            $"3PL quote via {_quoteProvider.ProviderName}",
            result.Price!.Value,
            ModificationType.Flat,
            Priority);
    }
}