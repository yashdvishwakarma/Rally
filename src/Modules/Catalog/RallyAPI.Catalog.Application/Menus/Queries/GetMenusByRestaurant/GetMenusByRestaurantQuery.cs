using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.Menus.Queries.GetMenusByRestaurant;

public sealed record GetMenusByRestaurantQuery(Guid RestaurantId)
    : IRequest<Result<List<MenuResponse>>>;

public sealed record MenuResponse(
    Guid Id,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);