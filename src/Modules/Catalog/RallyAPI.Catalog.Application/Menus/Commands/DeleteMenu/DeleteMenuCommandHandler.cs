using MediatR;
using RallyAPI.Catalog.Application.Abstractions;
using RallyAPI.Catalog.Domain.Menus;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.Menus.Commands.DeleteMenu;

internal sealed class DeleteMenuCommandHandler
    : IRequestHandler<DeleteMenuCommand, Result>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMenuCommandHandler(
        IMenuRepository menuRepository,
        IUnitOfWork unitOfWork)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteMenuCommand request,
        CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId, cancellationToken);

        if (menu is null)
            return Result.Failure(MenuErrors.NotFound);

        if (menu.RestaurantId != request.RestaurantId)
            return Result.Failure(MenuErrors.Unauthorized);

        _menuRepository.Delete(menu);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}