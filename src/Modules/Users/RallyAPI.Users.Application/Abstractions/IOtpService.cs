namespace RallyAPI.Users.Application.Abstractions;

public interface IOtpService
{
    Task<string> GenerateAndSendOtpAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<bool> VerifyOtpAsync(string phoneNumber, string otp, CancellationToken cancellationToken = default);
}