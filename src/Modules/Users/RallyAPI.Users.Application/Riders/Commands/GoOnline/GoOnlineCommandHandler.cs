using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;

namespace RallyAPI.Users.Application.Riders.Commands.GoOnline;

internal sealed class GoOnlineCommandHandler : IRequestHandler<GoOnlineCommand, Result>
{
    private readonly IRiderRepository _riderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GoOnlineCommandHandler(IRiderRepository riderRepository, IUnitOfWork unitOfWork)
    {
        _riderRepository = riderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(GoOnlineCommand request, CancellationToken cancellationToken)
    {
        var rider = await _riderRepository.GetByIdAsync(request.RiderId, cancellationToken);
        if (rider is null)
            return Result.Failure(Error.NotFound("Rider", request.RiderId));

        var result = rider.GoOnline();
        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}