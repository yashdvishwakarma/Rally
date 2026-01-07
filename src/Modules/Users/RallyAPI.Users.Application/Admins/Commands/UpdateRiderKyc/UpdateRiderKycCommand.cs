using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Domain.Enums;

namespace RallyAPI.Users.Application.Admins.Commands.UpdateRiderKyc;

public sealed record UpdateRiderKycCommand(
    Guid RequestedByAdminId,
    Guid RiderId,
    KycStatus NewKycStatus) : IRequest<Result>;