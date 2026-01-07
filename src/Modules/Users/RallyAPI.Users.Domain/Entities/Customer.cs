using RallyAPI.SharedKernel.Domain;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Domain.Events;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Domain.Entities;

public sealed class Customer : AggregateRoot
{
    public PhoneNumber Phone { get; private set; }
    public string? Name { get; private set; }
    public Email? Email { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<CustomerAddress> _addresses = new();
    public IReadOnlyCollection<CustomerAddress> Addresses => _addresses.AsReadOnly();

    // EF Core needs parameterless constructor
    private Customer() { }

    private Customer(PhoneNumber phone)
    {
        Phone = phone;
        IsActive = true;
        AddDomainEvent(new CustomerCreatedEvent(Id, phone.Value));
    }

    public static Result<Customer> Create(PhoneNumber phone)
    {
        // Business rules for customer creation go here
        var customer = new Customer(phone);
        return Result.Success(customer);
    }

    public Result UpdateProfile(string? name, Email? email)
    {
        if (name is not null)
        {
            if (name.Length > 100)
                return Result.Failure(Error.Validation("Name is too long."));
            Name = name.Trim();
        }

        Email = email;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result AddAddress(Address address)
    {
        if (_addresses.Count >= 10)
            return Result.Failure(Error.Validation("Maximum 10 addresses allowed."));

        // If this is first address or marked as default, handle it
        var isDefault = _addresses.Count == 0;
        
        var customerAddress = CustomerAddress.Create(Id, address, isDefault);
        _addresses.Add(customerAddress);
        MarkAsUpdated();
        
        return Result.Success();
    }

    public Result RemoveAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (address is null)
            return Result.Failure(Error.NotFound("Address", addressId));

        _addresses.Remove(address);
        
        // If removed address was default, make first one default
        if (address.IsDefault && _addresses.Any())
            _addresses.First().SetAsDefault();

        MarkAsUpdated();
        return Result.Success();
    }

    public Result SetDefaultAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (address is null)
            return Result.Failure(Error.NotFound("Address", addressId));

        foreach (var addr in _addresses)
            addr.UnsetDefault();

        address.SetAsDefault();
        MarkAsUpdated();
        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Failure(Error.Validation("Customer is already inactive."));

        IsActive = false;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result Activate()
    {
        if (IsActive)
            return Result.Failure(Error.Validation("Customer is already active."));

        IsActive = true;
        MarkAsUpdated();
        return Result.Success();
    }
}