using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Admins.Queries.ListRiders;

public sealed record ListRidersQuery(
    Guid RequestedByAdminId,
    bool? IsOnline,
    string? KycStatus,
    int Page = 1,
    int PageSize = 20) : IRequest<Result<ListRidersResponse>>;

public sealed record ListRidersResponse(
    List<RiderListItem> Riders,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record RiderListItem(
    Guid Id,
    string Phone,
    string Name,
    string VehicleType,
    string KycStatus,
    bool IsActive,
    bool IsOnline);