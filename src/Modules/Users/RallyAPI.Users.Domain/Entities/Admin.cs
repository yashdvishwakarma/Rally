using RallyAPI.SharedKernel.Domain;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Domain.Enums;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Domain.Entities;

public sealed class Admin : AggregateRoot
{
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Name { get; private set; }
    public AdminRole Role { get; private set; }
    public bool IsActive { get; private set; }

    // EF Core
    private Admin() { }

    private Admin(Email email, string passwordHash, string name, AdminRole role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Name = name;
        Role = role;
        IsActive = true;
    }

    public static Result<Admin> Create(Email email, string passwordHash, string name, AdminRole role)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Failure<Admin>(Error.Validation("Password is required."));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Admin>(Error.Validation("Name is required."));

        if (name.Length > 100)
            return Result.Failure<Admin>(Error.Validation("Name is too long."));

        return new Admin(email, passwordHash, name.Trim(), role);
    }

    public Result UpdateProfile(string? name)
    {
        if (name is not null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(Error.Validation("Name cannot be empty."));
            Name = name.Trim();
        }

        MarkAsUpdated();
        return Result.Success();
    }

    public Result UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            return Result.Failure(Error.Validation("Password cannot be empty."));

        PasswordHash = newPasswordHash;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result ChangeRole(AdminRole newRole)
    {
        Role = newRole;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Failure(Error.Validation("Admin is already inactive."));

        IsActive = false;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result Activate()
    {
        if (IsActive)
            return Result.Failure(Error.Validation("Admin is already active."));

        IsActive = true;
        MarkAsUpdated();
        return Result.Success();
    }
}