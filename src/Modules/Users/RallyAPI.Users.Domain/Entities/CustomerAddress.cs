using RallyAPI.SharedKernel.Domain;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Domain.Entities;

public sealed class CustomerAddress : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Address Address { get; private set; }
    public bool IsDefault { get; private set; }

    // EF Core
    private CustomerAddress() { }

    private CustomerAddress(Guid customerId, Address address, bool isDefault)
    {
        CustomerId = customerId;
        Address = address;
        IsDefault = isDefault;
    }

    internal static CustomerAddress Create(Guid customerId, Address address, bool isDefault)
    {
        return new CustomerAddress(customerId, address, isDefault);
    }

    internal void SetAsDefault()
    {
        IsDefault = true;
        MarkAsUpdated();
    }

    internal void UnsetDefault()
    {
        IsDefault = false;
        MarkAsUpdated();
    }

    public void UpdateAddress(Address newAddress)
    {
        Address = newAddress;
        MarkAsUpdated();
    }
}