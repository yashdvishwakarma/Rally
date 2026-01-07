using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Users.Domain.Events;

public sealed class RiderCreatedEvent : BaseDomainEvent
{
    public Guid RiderId { get; }
    public string PhoneNumber { get; }

    public RiderCreatedEvent(Guid riderId, string phoneNumber)
    {
        RiderId = riderId;
        PhoneNumber = phoneNumber;
    }
}