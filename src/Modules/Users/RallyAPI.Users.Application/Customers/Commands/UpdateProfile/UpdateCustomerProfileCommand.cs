using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Customers.Commands.UpdateProfile;

public sealed record UpdateCustomerProfileCommand(
    Guid CustomerId,
    string? Name,
    string? Email) : IRequest<Result>;