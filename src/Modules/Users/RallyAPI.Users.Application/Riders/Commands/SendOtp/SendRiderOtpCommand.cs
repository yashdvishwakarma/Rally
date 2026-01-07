using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Riders.Commands.SendOtp;

public sealed record SendRiderOtpCommand(string PhoneNumber) : IRequest<Result>;