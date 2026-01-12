using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RallyAPI.Users.Application.Customers.Commands.VerifyOtp;
using Microsoft.AspNetCore.Builder;

namespace RallyAPI.Users.Endpoints.Customers;

public class VerifyOtp : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/customers/otp/verify", HandleAsync)
            .WithName("CustomerVerifyOtp")
            .WithTags("Customers")
            .AllowAnonymous();
    }

    public record VerifyCustomerOtpRequest(string PhoneNumber, string Otp);

    private static async Task<IResult> HandleAsync(
        VerifyCustomerOtpRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new VerifyCustomerOtpCommand(request.PhoneNumber, request.Otp);
        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(new { error = result.Error.Message })
            : Results.Ok(result.Value);
    }
}