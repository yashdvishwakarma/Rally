using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.Enums;

namespace RallyAPI.Users.Application.Admins.Commands.UpdateRiderKyc;

internal sealed class UpdateRiderKycCommandHandler
    : IRequestHandler<UpdateRiderKycCommand, Result>
{
    private readonly IAdminRepository _adminRepository;
    private readonly IRiderRepository _riderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRiderKycCommandHandler(
        IAdminRepository adminRepository,
        IRiderRepository riderRepository,
        IUnitOfWork unitOfWork)
    {
        _adminRepository = adminRepository;
        _riderRepository = riderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateRiderKycCommand request,
        CancellationToken cancellationToken)
    {
        // Verify requesting admin
        var admin = await _adminRepository.GetByIdAsync(request.RequestedByAdminId, cancellationToken);
        if (admin is null)
            return Result.Failure(Error.NotFound("Admin", request.RequestedByAdminId));

        if (admin.Role == AdminRole.Support)
            return Result.Failure(Error.Forbidden("Support role cannot update KYC."));

        // Get rider
        var rider = await _riderRepository.GetByIdAsync(request.RiderId, cancellationToken);
        if (rider is null)
            return Result.Failure(Error.NotFound("Rider", request.RiderId));

        // Update KYC
        var result = rider.UpdateKycStatus(request.NewKycStatus);
        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}