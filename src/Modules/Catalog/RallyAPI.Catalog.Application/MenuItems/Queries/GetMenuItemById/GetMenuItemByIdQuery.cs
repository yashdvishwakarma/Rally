using MediatR;
using RallyAPI.Catalog.Application.MenuItems.Queries.GetMenuItemsByMenu;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.MenuItems.Queries.GetMenuItemById;

public sealed record GetMenuItemByIdQuery(Guid MenuItemId)
    : IRequest<Result<MenuItemResponse>>;