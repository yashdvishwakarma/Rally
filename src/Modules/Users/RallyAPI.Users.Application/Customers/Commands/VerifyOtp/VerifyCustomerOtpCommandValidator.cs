using FluentValidation;

namespace RallyAPI.Users.Application.Customers.Commands.VerifyOtp;

public sealed class VerifyCustomerOtpCommandValidator : AbstractValidator<VerifyCustomerOtpCommand>
{
    public VerifyCustomerOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.");

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required.")
            .Length(6).WithMessage("OTP must be 6 digits.");
    }
}