using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RallyAPI.Users.Application.Customers.Commands.SendOtp;
using Microsoft.AspNetCore.Builder;

namespace RallyAPI.Users.Endpoints.Customers;

public class SendOtp : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/customers/otp/send", HandleAsync)
            .WithName("CustomerSendOtp")
            .WithTags("Customers")
            .AllowAnonymous();
    }

    public record Request(string PhoneNumber);

    private static async Task<IResult> HandleAsync(
        Request request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new SendCustomerOtpCommand(request.PhoneNumber);
        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(new { error = result.Error.Message })
            : Results.Ok(new { message = "OTP sent successfully" });
    }
}