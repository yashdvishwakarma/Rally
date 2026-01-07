using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Admins.Queries.GetProfile;

public sealed record GetAdminProfileQuery(Guid AdminId)
    : IRequest<Result<AdminProfileResponse>>;

public sealed record AdminProfileResponse(
    Guid Id,
    string Email,
    string Name,
    string Role,
    bool IsActive);