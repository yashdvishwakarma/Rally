using RallyAPI.Orders.Domain.Enums;
using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderCancelledEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public CancellationReason Reason { get; }
    public Guid? CancelledBy { get; }

    public OrderCancelledEvent(
        Guid orderId,
        string orderNumber,
        CancellationReason reason,
        Guid? cancelledBy)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        Reason = reason;
        CancelledBy = cancelledBy;
    }
}