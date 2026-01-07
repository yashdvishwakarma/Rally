using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Restaurants.Commands.SetAvailability;

public sealed record SetRestaurantAvailabilityCommand(
    Guid RestaurantId,
    bool IsAcceptingOrders) : IRequest<Result>;