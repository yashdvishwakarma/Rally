using System.Text.RegularExpressions;
using RallyAPI.SharedKernel.Domain;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value.ToLowerInvariant();
    }

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Email>(Error.Validation("Email is required."));

        var trimmed = email.Trim().ToLowerInvariant();

        if (trimmed.Length > 255)
            return Result.Failure<Email>(Error.Validation("Email is too long."));

        // Basic email regex
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(trimmed, pattern))
            return Result.Failure<Email>(Error.Validation("Invalid email format."));

        return new Email(trimmed);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}