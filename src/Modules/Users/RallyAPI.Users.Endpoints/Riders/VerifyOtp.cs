using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using RallyAPI.Users.Application.Riders.Commands.VerifyOtp;

namespace RallyAPI.Users.Endpoints.Riders;

public class VerifyOtp : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/riders/otp/verify", HandleAsync)
            .WithTags("Riders")
            .WithSummary("Verify OTP and get token")
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleAsync(
        VerifyRiderOtpRequest request,
        ISender sender,
        CancellationToken ct)
    {
        var command = new VerifyRiderOtpCommand(request.PhoneNumber, request.Otp);
        var result = await sender.Send(command, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }
}

public record VerifyRiderOtpRequest(string PhoneNumber, string Otp);