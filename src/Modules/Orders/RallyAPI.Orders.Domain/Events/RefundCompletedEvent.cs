using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class RefundCompletedEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public decimal Amount { get; }

    public RefundCompletedEvent(
        Guid orderId,
        string orderNumber,
        decimal amount)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        Amount = amount;
    }
}