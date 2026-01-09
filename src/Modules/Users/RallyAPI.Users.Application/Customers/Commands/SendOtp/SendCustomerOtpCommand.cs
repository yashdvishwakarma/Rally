using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Customers.Commands.SendOtp;

public sealed record SendCustomerOtpCommand(string PhoneNumber) : IRequest<Result>;