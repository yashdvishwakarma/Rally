using MediatR;
using RallyAPI.Catalog.Application.Abstractions;
using RallyAPI.Catalog.Domain.Menus;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.Menus.Commands.UpdateMenu;

internal sealed class UpdateMenuCommandHandler
    : IRequestHandler<UpdateMenuCommand, Result>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMenuCommandHandler(
        IMenuRepository menuRepository,
        IUnitOfWork unitOfWork)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateMenuCommand request,
        CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId, cancellationToken);

        if (menu is null)
            return Result.Failure(MenuErrors.NotFound);

        if (menu.RestaurantId != request.RestaurantId)
            return Result.Failure(MenuErrors.Unauthorized);

        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure(MenuErrors.NameRequired);

        menu.Update(request.Name, request.Description, request.DisplayOrder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}