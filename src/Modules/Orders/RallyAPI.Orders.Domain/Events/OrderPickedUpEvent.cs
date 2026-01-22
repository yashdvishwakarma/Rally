using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderPickedUpEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid? RiderId { get; }

    public OrderPickedUpEvent(
        Guid orderId,
        string orderNumber,
        Guid? riderId)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        RiderId = riderId;
    }
}