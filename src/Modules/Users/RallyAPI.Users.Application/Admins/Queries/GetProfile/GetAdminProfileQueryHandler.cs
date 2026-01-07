using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;

namespace RallyAPI.Users.Application.Admins.Queries.GetProfile;

internal sealed class GetAdminProfileQueryHandler
    : IRequestHandler<GetAdminProfileQuery, Result<AdminProfileResponse>>
{
    private readonly IAdminRepository _adminRepository;

    public GetAdminProfileQueryHandler(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<Result<AdminProfileResponse>> Handle(
        GetAdminProfileQuery request,
        CancellationToken cancellationToken)
    {
        var admin = await _adminRepository.GetByIdAsync(request.AdminId, cancellationToken);
        if (admin is null)
            return Result.Failure<AdminProfileResponse>(Error.NotFound("Admin", request.AdminId));

        var response = new AdminProfileResponse(
            admin.Id,
            admin.Email.Value,
            admin.Name,
            admin.Role.ToString(),
            admin.IsActive);

        return Result.Success(response);
    }
}