using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.Menus.Commands.UpdateMenu;

public sealed record UpdateMenuCommand(
    Guid MenuId,
    Guid RestaurantId,
    string Name,
    string? Description,
    int DisplayOrder) : IRequest<Result>;