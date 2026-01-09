using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Customers.Commands.VerifyOtp;

public sealed record VerifyCustomerOtpCommand(string PhoneNumber, string Otp) : IRequest<Result<VerifyCustomerOtpResponse>>;

public sealed record VerifyCustomerOtpResponse(Guid CustomerId, string Token, bool IsNewCustomer);