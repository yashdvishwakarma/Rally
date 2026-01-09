using FluentValidation;

namespace RallyAPI.Users.Application.Customers.Commands.SendOtp;

public sealed class SendCustomerOtpCommandValidator : AbstractValidator<SendCustomerOtpCommand>
{
    public SendCustomerOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.");
    }
}