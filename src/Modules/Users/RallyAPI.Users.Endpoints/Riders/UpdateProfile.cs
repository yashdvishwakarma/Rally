using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RallyAPI.Users.Application.Riders.Commands.UpdateProfile;
using System.Security.Claims;

namespace RallyAPI.Users.Endpoints.Riders;

public class UpdateProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/riders/profile", HandleAsync)
            .WithTags("Riders")
            .WithSummary("Update rider profile")
            .RequireAuthorization("Rider");
    }

    private static async Task<IResult> HandleAsync(
        UpdateRiderProfileRequest request,
        ClaimsPrincipal user,
        ISender sender,
        CancellationToken ct)
    {
        var riderId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new UpdateRiderProfileCommand(
            riderId,
            request.Name,
            request.Email,
            request.VehicleNumber);

        var result = await sender.Send(command, ct);

        return result.IsSuccess
            ? Results.Ok(new { message = "Profile updated successfully" })
            : Results.BadRequest(result.Error);
    }
}

public record UpdateRiderProfileRequest(string? Name, string? Email, string? VehicleNumber);