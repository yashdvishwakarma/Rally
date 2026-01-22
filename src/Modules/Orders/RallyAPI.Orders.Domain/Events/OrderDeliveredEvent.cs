using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderDeliveredEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid CustomerId { get; }
    public Guid? RiderId { get; }

    public OrderDeliveredEvent(
        Guid orderId,
        string orderNumber,
        Guid customerId,
        Guid? riderId)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        RiderId = riderId;
    }
}