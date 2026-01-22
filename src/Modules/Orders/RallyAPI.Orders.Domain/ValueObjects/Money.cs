using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.ValueObjects;

/// <summary>
/// Represents a monetary amount with currency.
/// Encapsulates money operations to prevent calculation errors.
/// Default currency: INR (configurable for future expansion)
/// </summary>
public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Zero(string currency = "INR") => new(0, currency);

    public static Money FromDecimal(decimal amount, string currency = "INR")
    {
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required", nameof(currency));

        // Round to 2 decimal places
        return new Money(Math.Round(amount, 2), currency.ToUpperInvariant());
    }

    /// <summary>
    /// Creates money allowing negative amounts (for refunds, adjustments)
    /// </summary>
    public static Money FromDecimalAllowNegative(decimal amount, string currency = "INR")
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required", nameof(currency));

        return new Money(Math.Round(amount, 2), currency.ToUpperInvariant());
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor)
    {
        return new Money(Math.Round(Amount * factor, 2), Currency);
    }

    public Money Multiply(int quantity)
    {
        return new Money(Amount * quantity, Currency);
    }

    public bool IsZero() => Amount == 0;

    public bool IsPositive() => Amount > 0;

    public bool IsNegative() => Amount < 0;

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot operate on different currencies: {Currency} and {other.Currency}");
    }

    // Operator overloads for convenience
    public static Money operator +(Money a, Money b) => a.Add(b);
    public static Money operator -(Money a, Money b) => a.Subtract(b);
    public static Money operator *(Money a, decimal factor) => a.Multiply(factor);
    public static Money operator *(Money a, int quantity) => a.Multiply(quantity);
    public static bool operator >(Money a, Money b) => a.Amount > b.Amount;
    public static bool operator <(Money a, Money b) => a.Amount < b.Amount;
    public static bool operator >=(Money a, Money b) => a.Amount >= b.Amount;
    public static bool operator <=(Money a, Money b) => a.Amount <= b.Amount;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Currency} {Amount:N2}";

    /// <summary>
    /// Returns formatted string for display (e.g., "₹ 150.00")
    /// </summary>
    public string ToDisplayString() => Currency switch
    {
        "INR" => $"₹{Amount:N2}",
        "USD" => $"${Amount:N2}",
        "EUR" => $"€{Amount:N2}",
        _ => $"{Currency} {Amount:N2}"
    };
}