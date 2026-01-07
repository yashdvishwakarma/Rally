using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Application.Restaurants.Commands.Login;

internal sealed class LoginRestaurantCommandHandler
    : IRequestHandler<LoginRestaurantCommand, Result<LoginRestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginRestaurantCommandHandler(
        IRestaurantRepository restaurantRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _restaurantRepository = restaurantRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginRestaurantResponse>> Handle(
        LoginRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        // Validate email format
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<LoginRestaurantResponse>(emailResult.Error);

        // Find restaurant
        var restaurant = await _restaurantRepository.GetByEmailAsync(
            emailResult.Value,
            cancellationToken);

        if (restaurant is null)
            return Result.Failure<LoginRestaurantResponse>(
                Error.Validation("Invalid email or password."));

        // Verify password
        var isValidPassword = _passwordHasher.Verify(request.Password, restaurant.PasswordHash);
        if (!isValidPassword)
            return Result.Failure<LoginRestaurantResponse>(
                Error.Validation("Invalid email or password."));

        // Check if active
        if (!restaurant.IsActive)
            return Result.Failure<LoginRestaurantResponse>(
                Error.Validation("Restaurant account is inactive. Contact support."));

        // Generate token
        var token = _jwtProvider.GenerateRestaurantToken(restaurant);

        return new LoginRestaurantResponse(restaurant.Id, restaurant.Name, token);
    }
}