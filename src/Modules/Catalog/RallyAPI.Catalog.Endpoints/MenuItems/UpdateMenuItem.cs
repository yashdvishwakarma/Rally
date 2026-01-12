using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RallyAPI.Catalog.Application.MenuItems.Commands.CreateMenuItem;
using RallyAPI.Catalog.Application.MenuItems.Commands.UpdateMenuItem;

namespace RallyAPI.Catalog.Endpoints.MenuItems;

public class UpdateMenuItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/restaurant/items/{itemId:guid}", HandleAsync)
            .WithTags("Restaurant Menu Items")
            .WithSummary("Update a menu item")
            .RequireAuthorization("Restaurant");
    }

    private static async Task<IResult> HandleAsync(
        Guid itemId,
        UpdateMenuItemRequest request,
        ClaimsPrincipal user,
        ISender sender,
        CancellationToken ct)
    {
        var restaurantId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new UpdateMenuItemCommand(
            itemId,
            restaurantId,
            request.Name,
            request.Description,
            request.BasePrice,
            request.ImageUrl,
            request.DisplayOrder,
            request.IsVegetarian,
            request.PreparationTimeMinutes,
            request.Options?.Select(o => new MenuItemOptionDto(
                o.Name,
                o.Type,
                o.AdditionalPrice,
                o.IsDefault)).ToList());

        var result = await sender.Send(command, ct);

        return result.IsSuccess
            ? Results.Ok(new { message = "Menu item updated successfully" })
            : Results.BadRequest(result.Error);
    }
}

public record UpdateMenuItemRequest(
    string Name,
    string? Description,
    decimal BasePrice,
    string? ImageUrl,
    int DisplayOrder,
    bool IsVegetarian,
    int PreparationTimeMinutes,
    List<MenuItemOptionRequest>? Options);