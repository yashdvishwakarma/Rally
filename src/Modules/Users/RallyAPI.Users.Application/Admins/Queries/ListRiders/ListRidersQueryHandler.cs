using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;

namespace RallyAPI.Users.Application.Admins.Queries.ListRiders;

internal sealed class ListRidersQueryHandler
    : IRequestHandler<ListRidersQuery, Result<ListRidersResponse>>
{
    private readonly IAdminRepository _adminRepository;
    private readonly IRiderRepository _riderRepository;

    public ListRidersQueryHandler(
        IAdminRepository adminRepository,
        IRiderRepository riderRepository)
    {
        _adminRepository = adminRepository;
        _riderRepository = riderRepository;
    }

    public async Task<Result<ListRidersResponse>> Handle(
        ListRidersQuery request,
        CancellationToken cancellationToken)
    {
        // Verify admin exists
        var admin = await _adminRepository.GetByIdAsync(request.RequestedByAdminId, cancellationToken);
        if (admin is null)
            return Result.Failure<ListRidersResponse>(Error.NotFound("Admin", request.RequestedByAdminId));

        // Note: Full implementation will be in Infrastructure layer
        // This is a placeholder showing the pattern
        // Repository will handle filtering, pagination

        var riders = await _riderRepository.GetOnlineRidersAsync(cancellationToken);

        var riderItems = riders.Select(r => new RiderListItem(
            r.Id,
            r.Phone.GetFormatted(),
            r.Name,
            r.VehicleType.ToString(),
            r.KycStatus.ToString(),
            r.IsActive,
            r.IsOnline)).ToList();

        return new ListRidersResponse(
            riderItems,
            riderItems.Count,
            request.Page,
            request.PageSize);
    }
}