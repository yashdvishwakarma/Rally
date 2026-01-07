using FluentValidation;

namespace RallyAPI.Users.Application.Admins.Commands.CreateRider;

public sealed class CreateRiderCommandValidator : AbstractValidator<CreateRiderCommand>
{
    public CreateRiderCommandValidator()
    {
        RuleFor(x => x.RequestedByAdminId)
            .NotEmpty().WithMessage("Requesting admin ID is required.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name is too long.");

        RuleFor(x => x.VehicleType)
            .IsInEnum().WithMessage("Invalid vehicle type.");
    }
}