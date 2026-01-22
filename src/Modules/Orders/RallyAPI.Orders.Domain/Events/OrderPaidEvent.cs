using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderPaidEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid CustomerId { get; }
    public Guid RestaurantId { get; }
    public decimal Amount { get; }
    public int ItemCount { get; }

    public OrderPaidEvent(
        Guid orderId,
        string orderNumber,
        Guid customerId,
        Guid restaurantId,
        decimal amount,
        int itemCount)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        RestaurantId = restaurantId;
        Amount = amount;
        ItemCount = itemCount;
    }
}