using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Domain.Enums;

namespace RallyAPI.Users.Application.Admins.Commands.CreateAdmin;

public sealed record CreateAdminCommand(
    Guid RequestedByAdminId,
    string Email,
    string Password,
    string Name,
    AdminRole Role) : IRequest<Result<Guid>>;