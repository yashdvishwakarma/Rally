using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Riders.Commands.UpdateProfile;

public sealed record UpdateRiderProfileCommand(
    Guid RiderId,
    string? Name,
    string? Email,
    string? VehicleNumber) : IRequest<Result>;