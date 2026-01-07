using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Domain.Enums;

namespace RallyAPI.Users.Application.Admins.Commands.CreateRider;

public sealed record CreateRiderCommand(
    Guid RequestedByAdminId,
    string Phone,
    string Name,
    VehicleType VehicleType,
    string? VehicleNumber) : IRequest<Result<Guid>>;