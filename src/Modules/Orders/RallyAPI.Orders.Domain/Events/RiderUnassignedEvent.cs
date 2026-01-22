using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class RiderUnassignedEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public Guid PreviousRiderId { get; }

    public RiderUnassignedEvent(
        Guid orderId,
        string orderNumber,
        Guid previousRiderId)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        PreviousRiderId = previousRiderId;
    }
}