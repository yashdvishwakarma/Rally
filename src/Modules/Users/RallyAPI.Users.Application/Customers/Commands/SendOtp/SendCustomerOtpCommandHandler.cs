using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Application.Customers.Commands.SendOtp;

internal sealed class SendCustomerOtpCommandHandler : IRequestHandler<SendCustomerOtpCommand, Result>
{
    private readonly IOtpService _otpService;

    public SendCustomerOtpCommandHandler(IOtpService otpService)
    {
        _otpService = otpService;
    }

    public async Task<Result> Handle(SendCustomerOtpCommand request, CancellationToken cancellationToken)
    {
        // Validate phone number format
        var phoneResult = PhoneNumber.Create(request.PhoneNumber);
        if (phoneResult.IsFailure)
            return Result.Failure(phoneResult.Error);

        // Generate and send OTP
        await _otpService.GenerateAndSendOtpAsync(phoneResult.Value.Value, cancellationToken);

        return Result.Success();
    }
}