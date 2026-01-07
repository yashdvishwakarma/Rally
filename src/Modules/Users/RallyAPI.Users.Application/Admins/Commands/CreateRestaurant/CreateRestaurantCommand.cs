using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Admins.Commands.CreateRestaurant;

public sealed record CreateRestaurantCommand(
    Guid RequestedByAdminId,
    string Name,
    string Phone,
    string Email,
    string Password,
    string AddressLine,
    decimal Latitude,
    decimal Longitude) : IRequest<Result<Guid>>;