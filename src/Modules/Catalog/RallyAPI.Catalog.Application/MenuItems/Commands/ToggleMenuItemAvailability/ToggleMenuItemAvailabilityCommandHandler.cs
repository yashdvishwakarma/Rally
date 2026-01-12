using MediatR;
using RallyAPI.Catalog.Application.Abstractions;
using RallyAPI.Catalog.Domain.MenuItems;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.MenuItems.Commands.ToggleMenuItemAvailability;

internal sealed class ToggleMenuItemAvailabilityCommandHandler
    : IRequestHandler<ToggleMenuItemAvailabilityCommand, Result<ToggleMenuItemAvailabilityResponse>>
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleMenuItemAvailabilityCommandHandler(
        IMenuItemRepository menuItemRepository,
        IUnitOfWork unitOfWork)
    {
        _menuItemRepository = menuItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ToggleMenuItemAvailabilityResponse>> Handle(
        ToggleMenuItemAvailabilityCommand request,
        CancellationToken cancellationToken)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(
            request.MenuItemId,
            cancellationToken);

        if (menuItem is null)
            return Result.Failure<ToggleMenuItemAvailabilityResponse>(MenuItemErrors.NotFound);

        if (menuItem.RestaurantId != request.RestaurantId)
            return Result.Failure<ToggleMenuItemAvailabilityResponse>(MenuItemErrors.Unauthorized);

        menuItem.ToggleAvailability();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ToggleMenuItemAvailabilityResponse(menuItem.IsAvailable);
    }
}