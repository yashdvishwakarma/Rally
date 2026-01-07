using RallyAPI.SharedKernel.Domain;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Domain.Enums;
using RallyAPI.Users.Domain.Events;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Domain.Entities;

public sealed class Rider : AggregateRoot
{
    public PhoneNumber Phone { get; private set; }
    public string Name { get; private set; }
    public Email? Email { get; private set; }
    public VehicleType VehicleType { get; private set; }
    public string? VehicleNumber { get; private set; }
    public KycStatus KycStatus { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsOnline { get; private set; }
    public decimal? CurrentLatitude { get; private set; }
    public decimal? CurrentLongitude { get; private set; }
    public DateTime? LastLocationUpdate { get; private set; }

    // EF Core
    private Rider() { }

    private Rider(PhoneNumber phone, string name, VehicleType vehicleType)
    {
        Phone = phone;
        Name = name;
        VehicleType = vehicleType;
        KycStatus = KycStatus.Pending;
        IsActive = true;
        IsOnline = false;

        AddDomainEvent(new RiderCreatedEvent(Id, phone.Value));
    }

    public static Result<Rider> Create(PhoneNumber phone, string name, VehicleType vehicleType)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Rider>(Error.Validation("Rider name is required."));

        if (name.Length > 100)
            return Result.Failure<Rider>(Error.Validation("Rider name is too long."));

        return new Rider(phone, name.Trim(), vehicleType);
    }

    public Result UpdateProfile(string? name, Email? email, string? vehicleNumber)
    {
        if (name is not null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(Error.Validation("Name cannot be empty."));
            if (name.Length > 100)
                return Result.Failure(Error.Validation("Name is too long."));
            Name = name.Trim();
        }

        if (vehicleNumber is not null)
        {
            if (vehicleNumber.Length > 20)
                return Result.Failure(Error.Validation("Vehicle number is too long."));
            VehicleNumber = vehicleNumber.Trim().ToUpperInvariant();
        }

        Email = email;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result GoOnline()
    {
        if (!IsActive)
            return Result.Failure(Error.Validation("Inactive rider cannot go online."));

        if (KycStatus != KycStatus.Verified)
            return Result.Failure(Error.Validation("KYC must be verified to go online."));

        IsOnline = true;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result GoOffline()
    {
        IsOnline = false;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result UpdateLocation(decimal latitude, decimal longitude)
    {
        if (!IsOnline)
            return Result.Failure(Error.Validation("Rider must be online to update location."));

        // Basic India bounds validation
        if (latitude < 6 || latitude > 38)
            return Result.Failure(Error.Validation("Invalid latitude."));

        if (longitude < 68 || longitude > 98)
            return Result.Failure(Error.Validation("Invalid longitude."));

        CurrentLatitude = latitude;
        CurrentLongitude = longitude;
        LastLocationUpdate = DateTime.UtcNow;
        MarkAsUpdated();
        return Result.Success();
    }

    public Result UpdateKycStatus(KycStatus newStatus)
    {
        // Cannot go back to pending once verified/rejected
        if (KycStatus == KycStatus.Verified && newStatus == KycStatus.Pending)
            return Result.Failure(Error.Validation("Cannot revert verified KYC to pending."));

        KycStatus = newStatus;

        // If KYC rejected, force offline
        if (newStatus == KycStatus.Rejected)
            IsOnline = false;

        MarkAsUpdated();
        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Failure(Error.Validation("Rider is already inactive."));

        IsActive = false;
        IsOnline = false; // Force offline
        MarkAsUpdated();
        return Result.Success();
    }

    public Result Activate()
    {
        if (IsActive)
            return Result.Failure(Error.Validation("Rider is already active."));

        IsActive = true;
        MarkAsUpdated();
        return Result.Success();
    }
}