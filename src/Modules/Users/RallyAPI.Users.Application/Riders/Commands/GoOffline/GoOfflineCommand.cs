using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Riders.Commands.GoOffline;

public sealed record GoOfflineCommand(Guid RiderId) : IRequest<Result>;