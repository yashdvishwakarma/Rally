using MediatR;
using RallyAPI.Catalog.Application.Abstractions;
using RallyAPI.Catalog.Domain.Menus;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Application.Menus.Commands.CreateMenu;

internal sealed class CreateMenuCommandHandler
    : IRequestHandler<CreateMenuCommand, Result<CreateMenuResponse>>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMenuCommandHandler(
        IMenuRepository menuRepository,
        IUnitOfWork unitOfWork)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateMenuResponse>> Handle(
        CreateMenuCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure<CreateMenuResponse>(MenuErrors.NameRequired);

        var menu = Menu.Create(
            request.RestaurantId,
            request.Name,
            request.Description,
            request.DisplayOrder);

        _menuRepository.Add(menu);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateMenuResponse(menu.Id);
    }
}