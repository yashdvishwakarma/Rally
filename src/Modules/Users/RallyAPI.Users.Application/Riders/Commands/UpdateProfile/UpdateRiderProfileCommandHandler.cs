using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Application.Riders.Commands.UpdateProfile;

internal sealed class UpdateRiderProfileCommandHandler
    : IRequestHandler<UpdateRiderProfileCommand, Result>
{
    private readonly IRiderRepository _riderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRiderProfileCommandHandler(
        IRiderRepository riderRepository,
        IUnitOfWork unitOfWork)
    {
        _riderRepository = riderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateRiderProfileCommand request,
        CancellationToken cancellationToken)
    {
        var rider = await _riderRepository.GetByIdAsync(request.RiderId, cancellationToken);
        if (rider is null)
            return Result.Failure(Error.NotFound("Rider", request.RiderId));

        Email? email = null;
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure)
                return Result.Failure(emailResult.Error);
            email = emailResult.Value;
        }

        var result = rider.UpdateProfile(request.Name, email, request.VehicleNumber);
        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}