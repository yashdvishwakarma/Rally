using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Users.Domain.Events;

public sealed class CustomerCreatedEvent : BaseDomainEvent
{
    public Guid CustomerId { get; }
    public string PhoneNumber { get; }

    public CustomerCreatedEvent(Guid customerId, string phoneNumber)
    {
        CustomerId = customerId;
        PhoneNumber = phoneNumber;
    }
}