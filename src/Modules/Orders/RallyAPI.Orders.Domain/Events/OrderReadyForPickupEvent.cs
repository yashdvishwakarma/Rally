using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderReadyForPickupEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid RestaurantId { get; }

    public OrderReadyForPickupEvent(
        Guid orderId,
        string orderNumber,
        Guid restaurantId)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        RestaurantId = restaurantId;
    }
}