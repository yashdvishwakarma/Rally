using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Riders.Commands.UpdateLocation;

public sealed record UpdateRiderLocationCommand(
    Guid RiderId,
    decimal Latitude,
    decimal Longitude) : IRequest<Result>;