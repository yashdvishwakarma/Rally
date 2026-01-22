using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderFailedEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public string? Reason { get; }

    public OrderFailedEvent(
        Guid orderId,
        string orderNumber,
        string? reason)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        Reason = reason;
    }
}