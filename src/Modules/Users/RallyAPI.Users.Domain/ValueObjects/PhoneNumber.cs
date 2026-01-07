using System.Text.RegularExpressions;
using RallyAPI.SharedKernel.Domain;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return Result.Failure<PhoneNumber>(Error.Validation("Phone number is required."));

        // Remove spaces, dashes
        var cleaned = Regex.Replace(phone, @"[\s\-]", "");

        // Indian phone: 10 digits, optionally with +91
        if (cleaned.StartsWith("+91"))
            cleaned = cleaned.Substring(3);

        if (cleaned.Length != 10)
            return Result.Failure<PhoneNumber>(Error.Validation("Phone number must be 10 digits."));

        if (!Regex.IsMatch(cleaned, @"^[6-9]\d{9}$"))
            return Result.Failure<PhoneNumber>(Error.Validation("Invalid Indian phone number."));

        return new PhoneNumber(cleaned);
    }

    public string GetFormatted() => $"+91{Value}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => GetFormatted();
}