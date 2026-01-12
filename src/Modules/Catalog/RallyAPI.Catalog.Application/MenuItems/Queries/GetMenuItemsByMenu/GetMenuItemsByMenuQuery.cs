using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.MenuItems.Queries.GetMenuItemsByMenu;

public sealed record GetMenuItemsByMenuQuery(Guid MenuId)
    : IRequest<Result<List<MenuItemResponse>>>;

public sealed record MenuItemResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal BasePrice,
    string? ImageUrl,
    bool IsAvailable,
    bool IsVegetarian,
    int PreparationTimeMinutes,
    List<MenuItemOptionResponse> Options);

public sealed record MenuItemOptionResponse(
    Guid Id,
    string Name,
    string Type,
    decimal AdditionalPrice,
    bool IsDefault);