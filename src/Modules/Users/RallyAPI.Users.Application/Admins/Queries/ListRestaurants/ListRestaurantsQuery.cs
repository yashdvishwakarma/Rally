using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Admins.Queries.ListRestaurants;

public sealed record ListRestaurantsQuery(
    Guid RequestedByAdminId,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20) : IRequest<Result<ListRestaurantsResponse>>;

public sealed record ListRestaurantsResponse(
    List<RestaurantListItem> Restaurants,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record RestaurantListItem(
    Guid Id,
    string Name,
    string Phone,
    string Email,
    bool IsActive,
    bool IsAcceptingOrders);