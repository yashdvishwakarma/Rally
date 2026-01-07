using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Admins.Commands.Login;

public sealed record LoginAdminCommand(
    string Email,
    string Password) : IRequest<Result<LoginAdminResponse>>;

public sealed record LoginAdminResponse(
    Guid AdminId,
    string Name,
    string Role,
    string Token);