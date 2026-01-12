using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.MenuItems.Commands.ToggleMenuItemAvailability;

public sealed record ToggleMenuItemAvailabilityCommand(
    Guid MenuItemId,
    Guid RestaurantId) : IRequest<Result<ToggleMenuItemAvailabilityResponse>>;

public sealed record ToggleMenuItemAvailabilityResponse(bool IsAvailable);