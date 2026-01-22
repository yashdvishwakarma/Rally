using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderRejectedEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid RestaurantId { get; }
    public Guid CustomerId { get; }
    public string? Reason { get; }

    public OrderRejectedEvent(
        Guid orderId,
        string orderNumber,
        Guid restaurantId,
        Guid customerId,
        string? reason)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        RestaurantId = restaurantId;
        CustomerId = customerId;
        Reason = reason;
    }
}