using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderConfirmedEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid RestaurantId { get; }
    public Guid CustomerId { get; }

    public OrderConfirmedEvent(
        Guid orderId,
        string orderNumber,
        Guid restaurantId,
        Guid customerId)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        RestaurantId = restaurantId;
        CustomerId = customerId;
    }
}