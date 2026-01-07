using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Restaurants.Commands.UpdateProfile;

public sealed record UpdateRestaurantProfileCommand(
    Guid RestaurantId,
    string? Name,
    string? AddressLine,
    string? Phone) : IRequest<Result>;