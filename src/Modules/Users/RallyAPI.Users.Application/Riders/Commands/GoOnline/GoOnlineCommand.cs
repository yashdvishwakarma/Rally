using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Riders.Commands.GoOnline;

public sealed record GoOnlineCommand(Guid RiderId) : IRequest<Result>;