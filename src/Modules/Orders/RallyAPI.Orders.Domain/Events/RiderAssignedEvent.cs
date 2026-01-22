using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class RiderAssignedEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid RiderId { get; }

    public RiderAssignedEvent(
        Guid orderId,
        string orderNumber,
        Guid riderId)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        RiderId = riderId;
    }
}