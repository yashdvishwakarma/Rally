using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.Entities;
using RallyAPI.Users.Domain.Enums;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Application.Admins.Commands.CreateRider;

internal sealed class CreateRiderCommandHandler
    : IRequestHandler<CreateRiderCommand, Result<Guid>>
{
    private readonly IAdminRepository _adminRepository;
    private readonly IRiderRepository _riderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRiderCommandHandler(
        IAdminRepository adminRepository,
        IRiderRepository riderRepository,
        IUnitOfWork unitOfWork)
    {
        _adminRepository = adminRepository;
        _riderRepository = riderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateRiderCommand request,
        CancellationToken cancellationToken)
    {
        // Verify requesting admin exists and has permission
        var admin = await _adminRepository.GetByIdAsync(request.RequestedByAdminId, cancellationToken);
        if (admin is null)
            return Result.Failure<Guid>(Error.NotFound("Admin", request.RequestedByAdminId));

        if (admin.Role == AdminRole.Support)
            return Result.Failure<Guid>(Error.Forbidden("Support role cannot create riders."));

        // Validate phone
        var phoneResult = PhoneNumber.Create(request.Phone);
        if (phoneResult.IsFailure)
            return Result.Failure<Guid>(phoneResult.Error);

        // Check if phone already exists
        var exists = await _riderRepository.ExistsByPhoneAsync(phoneResult.Value, cancellationToken);
        if (exists)
            return Result.Failure<Guid>(Error.Conflict("Rider with this phone already exists."));

        // Create rider
        var riderResult = Rider.Create(
            phoneResult.Value,
            request.Name,
            request.VehicleType);

        if (riderResult.IsFailure)
            return Result.Failure<Guid>(riderResult.Error);

        var rider = riderResult.Value;

        // Update vehicle number if provided
        if (!string.IsNullOrWhiteSpace(request.VehicleNumber))
        {
            rider.UpdateProfile(null, null, request.VehicleNumber);
        }

        await _riderRepository.AddAsync(rider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(rider.Id);
    }
}