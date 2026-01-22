using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class OrderPreparingEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }

    public OrderPreparingEvent(Guid orderId, string orderNumber)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
    }
}