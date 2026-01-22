using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Events;

public sealed class RefundInitiatedEvent : BaseDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public decimal Amount { get; }

    public RefundInitiatedEvent(
        Guid orderId,
        string orderNumber,
        decimal amount)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        Amount = amount;
    }
}