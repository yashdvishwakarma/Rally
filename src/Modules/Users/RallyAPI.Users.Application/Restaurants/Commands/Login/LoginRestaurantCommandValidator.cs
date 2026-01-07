using FluentValidation;

namespace RallyAPI.Users.Application.Restaurants.Commands.Login;

public sealed class LoginRestaurantCommandValidator : AbstractValidator<LoginRestaurantCommand>
{
    public LoginRestaurantCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}