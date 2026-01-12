using MediatR;
using RallyAPI.Catalog.Application.Abstractions;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.Menus.Queries.GetMenusByRestaurant;

internal sealed class GetMenusByRestaurantQueryHandler
    : IRequestHandler<GetMenusByRestaurantQuery, Result<List<MenuResponse>>>
{
    private readonly IMenuRepository _menuRepository;

    public GetMenusByRestaurantQueryHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<Result<List<MenuResponse>>> Handle(
        GetMenusByRestaurantQuery request,
        CancellationToken cancellationToken)
    {
        var menus = await _menuRepository.GetByRestaurantIdAsync(
            request.RestaurantId,
            cancellationToken);

        var response = menus
            .OrderBy(m => m.DisplayOrder)
            .Select(m => new MenuResponse(
                m.Id,
                m.Name,
                m.Description,
                m.DisplayOrder,
                m.IsActive))
            .ToList();

        return response;
    }
}