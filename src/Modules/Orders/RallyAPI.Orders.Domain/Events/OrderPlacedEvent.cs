using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderPlacedEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid CustomerId { get; }
    public Guid RestaurantId { get; }
    public decimal TotalAmount { get; }
    public int ItemCount { get; }

    public OrderPlacedEvent(
        Guid orderId,
        string orderNumber,
        Guid customerId,
        Guid restaurantId,
        decimal totalAmount,
        int itemCount)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        RestaurantId = restaurantId;
        TotalAmount = totalAmount;
        ItemCount = itemCount;
    }
}