using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.Entities;
using RallyAPI.Users.Domain.Enums;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Application.Admins.Commands.CreateRestaurant;

internal sealed class CreateRestaurantCommandHandler
    : IRequestHandler<CreateRestaurantCommand, Result<Guid>>
{
    private readonly IAdminRepository _adminRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRestaurantCommandHandler(
        IAdminRepository adminRepository,
        IRestaurantRepository restaurantRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _adminRepository = adminRepository;
        _restaurantRepository = restaurantRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        // Verify requesting admin
        var admin = await _adminRepository.GetByIdAsync(request.RequestedByAdminId, cancellationToken);
        if (admin is null)
            return Result.Failure<Guid>(Error.NotFound("Admin", request.RequestedByAdminId));

        if (admin.Role == AdminRole.Support)
            return Result.Failure<Guid>(Error.Forbidden("Support role cannot create restaurants."));

        // Validate phone
        var phoneResult = PhoneNumber.Create(request.Phone);
        if (phoneResult.IsFailure)
            return Result.Failure<Guid>(phoneResult.Error);

        // Validate email
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<Guid>(emailResult.Error);

        // Check if email already exists
        var exists = await _restaurantRepository.ExistsByEmailAsync(emailResult.Value, cancellationToken);
        if (exists)
            return Result.Failure<Guid>(Error.Conflict("Restaurant with this email already exists."));

        // Hash password
        var passwordHash = _passwordHasher.Hash(request.Password);

        // Create restaurant
        var restaurantResult = Restaurant.Create(
            request.Name,
            phoneResult.Value,
            emailResult.Value,
            passwordHash,
            request.AddressLine,
            request.Latitude,
            request.Longitude);

        if (restaurantResult.IsFailure)
            return Result.Failure<Guid>(restaurantResult.Error);

        await _restaurantRepository.AddAsync(restaurantResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(restaurantResult.Value.Id);
    }
}