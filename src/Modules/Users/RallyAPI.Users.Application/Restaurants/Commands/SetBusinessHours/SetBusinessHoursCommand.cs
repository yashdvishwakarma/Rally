using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Restaurants.Commands.SetBusinessHours;

public sealed record SetBusinessHoursCommand(
    Guid RestaurantId,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime) : IRequest<Result>;