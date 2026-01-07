using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;

namespace RallyAPI.Users.Application.Admins.Queries.ListRestaurants;

internal sealed class ListRestaurantsQueryHandler
    : IRequestHandler<ListRestaurantsQuery, Result<ListRestaurantsResponse>>
{
    private readonly IAdminRepository _adminRepository;
    private readonly IRestaurantRepository _restaurantRepository;

    public ListRestaurantsQueryHandler(
        IAdminRepository adminRepository,
        IRestaurantRepository restaurantRepository)
    {
        _adminRepository = adminRepository;
        _restaurantRepository = restaurantRepository;
    }

    public async Task<Result<ListRestaurantsResponse>> Handle(
        ListRestaurantsQuery request,
        CancellationToken cancellationToken)
    {
        var admin = await _adminRepository.GetByIdAsync(request.RequestedByAdminId, cancellationToken);
        if (admin is null)
            return Result.Failure<ListRestaurantsResponse>(Error.NotFound("Admin", request.RequestedByAdminId));

        // Placeholder - full implementation in Infrastructure
        // Will add GetAllAsync with filters to repository

        return new ListRestaurantsResponse(
            new List<RestaurantListItem>(),
            0,
            request.Page,
            request.PageSize);
    }
}