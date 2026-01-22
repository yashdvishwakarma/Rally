using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.ValueObjects;

/// <summary>
/// Strongly-typed Order identifier.
/// Prevents mixing up IDs from different entities.
/// </summary>
public sealed class OrderId : ValueObject
{
    public Guid Value { get; }

    private OrderId(Guid value)
    {
        Value = value;
    }

    public static OrderId Create() => new(Guid.NewGuid());

    public static OrderId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("OrderId cannot be empty", nameof(value));

        return new OrderId(value);
    }

    public static implicit operator Guid(OrderId orderId) => orderId.Value;

    public static explicit operator OrderId(Guid value) => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}