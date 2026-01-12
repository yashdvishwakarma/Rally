using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.MenuItems.Commands.CreateMenuItem;

public sealed record CreateMenuItemCommand(
    Guid RestaurantId,
    Guid MenuId,
    string Name,
    string? Description,
    decimal BasePrice,
    string? ImageUrl,
    int DisplayOrder,
    bool IsVegetarian,
    int PreparationTimeMinutes,
    List<MenuItemOptionDto>? Options) : IRequest<Result<CreateMenuItemResponse>>;

public sealed record MenuItemOptionDto(
    string Name,
    string Type,
    decimal AdditionalPrice,
    bool IsDefault);

public sealed record CreateMenuItemResponse(Guid MenuItemId);