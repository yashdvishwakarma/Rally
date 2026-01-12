using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.Menus.Commands.DeleteMenu;

public sealed record DeleteMenuCommand(
    Guid MenuId,
    Guid RestaurantId) : IRequest<Result>;