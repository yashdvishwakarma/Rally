using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.ValueObjects;

/// <summary>
/// Human-readable order number for customer reference.
/// Format: ORD-{YYYYMMDD}-{SEQUENCE}
/// Example: ORD-20240115-00042
/// </summary>
public sealed class OrderNumber : ValueObject
{
    public string Value { get; }

    private OrderNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates order number with provided sequence.
    /// Sequence should come from a sequence generator service.
    /// </summary>
    public static OrderNumber Create(int dailySequence, DateTime? date = null)
    {
        var orderDate = date ?? DateTime.UtcNow;
        var formatted = $"ORD-{orderDate:yyyyMMdd}-{dailySequence:D5}";
        return new OrderNumber(formatted);
    }

    /// <summary>
    /// Creates order number from existing string (for reconstitution from DB)
    /// </summary>
    public static OrderNumber From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Order number cannot be empty", nameof(value));

        return new OrderNumber(value);
    }

    /// <summary>
    /// Generates a fallback order number using timestamp + random (use only if sequence unavailable)
    /// </summary>
    public static OrderNumber CreateFallback()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = Random.Shared.Next(1000, 9999);
        return new OrderNumber($"ORD-{timestamp}-{random}");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(OrderNumber orderNumber) => orderNumber.Value;
}