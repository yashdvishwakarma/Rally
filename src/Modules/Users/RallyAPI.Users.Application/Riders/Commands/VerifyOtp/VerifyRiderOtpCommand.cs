using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Riders.Commands.VerifyOtp;

public sealed record VerifyRiderOtpCommand(string PhoneNumber, string Otp) : IRequest<Result<VerifyRiderOtpResponse>>;

public sealed record VerifyRiderOtpResponse(Guid RiderId, string Token, string KycStatus);