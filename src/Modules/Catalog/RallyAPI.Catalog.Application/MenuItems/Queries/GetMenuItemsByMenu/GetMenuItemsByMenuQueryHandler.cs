using MediatR;
using RallyAPI.Catalog.Application.Abstractions;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.MenuItems.Queries.GetMenuItemsByMenu;

internal sealed class GetMenuItemsByMenuQueryHandler
    : IRequestHandler<GetMenuItemsByMenuQuery, Result<List<MenuItemResponse>>>
{
    private readonly IMenuItemRepository _menuItemRepository;

    public GetMenuItemsByMenuQueryHandler(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<Result<List<MenuItemResponse>>> Handle(
        GetMenuItemsByMenuQuery request,
        CancellationToken cancellationToken)
    {
        var items = await _menuItemRepository.GetByMenuIdAsync(
            request.MenuId,
            cancellationToken);

        var response = items
            .OrderBy(i => i.DisplayOrder)
            .Select(i => new MenuItemResponse(
                i.Id,
                i.Name,
                i.Description,
                i.BasePrice,
                i.ImageUrl,
                i.IsAvailable,
                i.IsVegetarian,
                i.PreparationTimeMinutes,
                i.Options.Select(o => new MenuItemOptionResponse(
                    o.Id,
                    o.Name,
                    o.Type.ToString(),
                    o.AdditionalPrice,
                    o.IsDefault)).ToList()))
            .ToList();

        return response;
    }
}