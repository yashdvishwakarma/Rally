using System.Collections.Concurrent;
using RallyAPI.Users.Application.Abstractions;

namespace RallyAPI.Users.Infrastructure.Services;

public class OtpService : IOtpService
{
    // In-memory store for MVP (use Redis in production)
    private static readonly ConcurrentDictionary<string, OtpEntry> _otpStore = new();

    // OTP validity duration
    private static readonly TimeSpan OtpValidity = TimeSpan.FromMinutes(5);

    public Task<string> GenerateAndSendOtpAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        // Generate 6-digit OTP
        var otp = GenerateOtp();

        // Store with expiration
        var entry = new OtpEntry
        {
            Otp = otp,
            ExpiresAt = DateTime.UtcNow.Add(OtpValidity),
            Attempts = 0
        };

        _otpStore.AddOrUpdate(phoneNumber, entry, (_, _) => entry);

        // TODO: In production, integrate with SMS provider (Twilio, MSG91, etc.)
        // For MVP, we just log to console
        Console.WriteLine($"========================================");
        Console.WriteLine($"[OTP SERVICE] Phone: {phoneNumber}");
        Console.WriteLine($"[OTP SERVICE] OTP: {otp}");
        Console.WriteLine($"[OTP SERVICE] Valid until: {entry.ExpiresAt:HH:mm:ss}");
        Console.WriteLine($"========================================");

        return Task.FromResult(otp);
    }

    public Task<bool> VerifyOtpAsync(string phoneNumber, string otp, CancellationToken cancellationToken = default)
    {
        // Check if OTP exists for this phone
        if (!_otpStore.TryGetValue(phoneNumber, out var entry))
        {
            Console.WriteLine($"[OTP SERVICE] No OTP found for {phoneNumber}");
            return Task.FromResult(false);
        }

        // Check if expired
        if (DateTime.UtcNow > entry.ExpiresAt)
        {
            _otpStore.TryRemove(phoneNumber, out _);
            Console.WriteLine($"[OTP SERVICE] OTP expired for {phoneNumber}");
            return Task.FromResult(false);
        }

        // Check attempt limit (prevent brute force)
        if (entry.Attempts >= 3)
        {
            _otpStore.TryRemove(phoneNumber, out _);
            Console.WriteLine($"[OTP SERVICE] Too many attempts for {phoneNumber}");
            return Task.FromResult(false);
        }

        // Increment attempts
        entry.Attempts++;

        // Verify OTP
        if (entry.Otp != otp)
        {
            Console.WriteLine($"[OTP SERVICE] Invalid OTP for {phoneNumber}. Attempt {entry.Attempts}/3");
            return Task.FromResult(false);
        }

        // Success - remove OTP (one-time use)
        _otpStore.TryRemove(phoneNumber, out _);
        Console.WriteLine($"[OTP SERVICE] OTP verified for {phoneNumber}");
        return Task.FromResult(true);
    }

    private static string GenerateOtp()
    {
        // For development/testing, use a fixed OTP
#if DEBUG
        return "123456";
#else
        var random = new Random();
        return random.Next(100000, 999999).ToString();
#endif
    }

    private class OtpEntry
    {
        public string Otp { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int Attempts { get; set; }
    }
}