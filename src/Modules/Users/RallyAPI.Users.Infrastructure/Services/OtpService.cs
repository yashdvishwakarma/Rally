//using System.Collections.Concurrent;
//using RallyAPI.Users.Application.Abstractions;

//namespace RallyAPI.Users.Infrastructure.Services;

//public class OtpService : IOtpService
//{
//    // In-memory store for MVP (use Redis in production)
//    private static readonly ConcurrentDictionary<string, OtpEntry> _otpStore = new();

//    // OTP validity duration
//    private static readonly TimeSpan OtpValidity = TimeSpan.FromMinutes(5);

//    public Task<string> GenerateAndSendOtpAsync(string phoneNumber, CancellationToken cancellationToken = default)
//    {
//        // Generate 6-digit OTP
//        var otp = GenerateOtp();

//        // Store with expiration
//        var entry = new OtpEntry
//        {
//            Otp = otp,
//            ExpiresAt = DateTime.UtcNow.Add(OtpValidity),
//            Attempts = 0
//        };

//        _otpStore.AddOrUpdate(phoneNumber, entry, (_, _) => entry);

//        // TODO: In production, integrate with SMS provider (Twilio, MSG91, etc.)
//        // For MVP, we just log to console
//        Console.WriteLine($"========================================");
//        Console.WriteLine($"[OTP SERVICE] Phone: {phoneNumber}");
//        Console.WriteLine($"[OTP SERVICE] OTP: {otp}");
//        Console.WriteLine($"[OTP SERVICE] Valid until: {entry.ExpiresAt:HH:mm:ss}");
//        Console.WriteLine($"========================================");

//        return Task.FromResult(otp);
//    }

//    public Task<bool> VerifyOtpAsync(string phoneNumber, string otp, CancellationToken cancellationToken = default)
//    {
//        // Check if OTP exists for this phone
//        if (!_otpStore.TryGetValue(phoneNumber, out var entry))
//        {
//            Console.WriteLine($"[OTP SERVICE] No OTP found for {phoneNumber}");
//            return Task.FromResult(false);
//        }

//        // Check if expired
//        if (DateTime.UtcNow > entry.ExpiresAt)
//        {
//            _otpStore.TryRemove(phoneNumber, out _);
//            Console.WriteLine($"[OTP SERVICE] OTP expired for {phoneNumber}");
//            return Task.FromResult(false);
//        }

//        // Check attempt limit (prevent brute force)
//        if (entry.Attempts >= 3)
//        {
//            _otpStore.TryRemove(phoneNumber, out _);
//            Console.WriteLine($"[OTP SERVICE] Too many attempts for {phoneNumber}");
//            return Task.FromResult(false);
//        }

//        // Increment attempts
//        entry.Attempts++;

//        // Verify OTP
//        if (entry.Otp != otp)
//        {
//            Console.WriteLine($"[OTP SERVICE] Invalid OTP for {phoneNumber}. Attempt {entry.Attempts}/3");
//            return Task.FromResult(false);
//        }

//        // Success - remove OTP (one-time use)
//        _otpStore.TryRemove(phoneNumber, out _);
//        Console.WriteLine($"[OTP SERVICE] OTP verified for {phoneNumber}");
//        return Task.FromResult(true);
//    }

//    private static string GenerateOtp()
//    {
//        // For development/testing, use a fixed OTP
//#if DEBUG
//        return "123456";
//#else
//        var random = new Random();
//        return random.Next(100000, 999999).ToString();
//#endif
//    }

//    private class OtpEntry
//    {
//        public string Otp { get; set; } = string.Empty;
//        public DateTime ExpiresAt { get; set; }
//        public int Attempts { get; set; }
//    }
//}

using MediatR;
using RallyAPI.Users.Application.Abstractions;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

namespace RallyAPI.Users.Infrastructure.Services;

public class OtpService : IOtpService
{
    private readonly IDatabase _redis;

    // Configuration
    private const int OtpExpiryMinutes = 5;
    private const int MaxAttempts = 3;
    private const int LockoutMinutes = 15;
    private const int RateLimitPerPhone = 3;      // max 3 OTPs per phone per 10 min
    private const int RateLimitWindowMinutes = 10;

    public OtpService(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task<string> GenerateAndSendOtpAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var phoneKey = NormalizePhone(phoneNumber);

        // Check if phone is locked out (too many failed attempts)
        if (await _redis.KeyExistsAsync($"otp:lock:{phoneKey}"))
        {
            throw new InvalidOperationException(
                "Too many failed attempts. Try again in 15 minutes.");
        }

        // Rate limit: max 3 OTP requests per 10 minutes per phone
        var rateKey = $"otp:rate:{phoneKey}";
        var currentRate = await _redis.StringGetAsync(rateKey);
        if (currentRate.HasValue && (int)currentRate >= RateLimitPerPhone)
        {
            throw new InvalidOperationException(
                "Too many OTP requests. Please wait before trying again.");
        }

        // Generate OTP
        var otp = GenerateOtp();

        // Store hashed OTP in Redis with expiry
        var hashedOtp = HashOtp(otp);
        await _redis.StringSetAsync(
            $"otp:code:{phoneKey}",
            hashedOtp,
            TimeSpan.FromMinutes(OtpExpiryMinutes));

        // Reset attempt counter
        await _redis.StringSetAsync(
            $"otp:attempts:{phoneKey}",
            0,
            TimeSpan.FromMinutes(OtpExpiryMinutes));

        // Increment rate limit counter
        await _redis.StringIncrementAsync(rateKey);
        // Set expiry only if this is the first request in the window
        if (!currentRate.HasValue)
        {
            await _redis.KeyExpireAsync(rateKey, TimeSpan.FromMinutes(RateLimitWindowMinutes));
        }

        // TODO: Send via SMS provider (Twilio/MSG91)
        // For now, log to console
        Console.WriteLine($"========================================");
        Console.WriteLine($"[OTP SERVICE - REDIS] Phone: {phoneNumber}");
        Console.WriteLine($"[OTP SERVICE - REDIS] OTP: {otp}");
        Console.WriteLine($"[OTP SERVICE - REDIS] Expires in: {OtpExpiryMinutes} min");
        Console.WriteLine($"========================================");

        return otp;
    }

    public async Task<bool> VerifyOtpAsync(
        string phoneNumber,
        string otp,
        CancellationToken cancellationToken = default)
    {
        var phoneKey = NormalizePhone(phoneNumber);

        // Check lockout
        if (await _redis.KeyExistsAsync($"otp:lock:{phoneKey}"))
            return false;

        // Get stored OTP hash
        var storedHash = await _redis.StringGetAsync($"otp:code:{phoneKey}");
        if (!storedHash.HasValue)
            return false; // No OTP exists or it expired

        // Check attempts
        var attempts = (int)await _redis.StringIncrementAsync($"otp:attempts:{phoneKey}");
        if (attempts > MaxAttempts)
        {
            // Lock the phone number
            await _redis.StringSetAsync(
                $"otp:lock:{phoneKey}",
                "locked",
                TimeSpan.FromMinutes(LockoutMinutes));

            // Clean up OTP keys
            await CleanupKeys(phoneKey);

            Console.WriteLine($"[OTP SERVICE] Phone {phoneNumber} locked for {LockoutMinutes} min");
            return false;
        }

        // Verify
        var hashedInput = HashOtp(otp);
        if (storedHash != hashedInput)
        {
            Console.WriteLine($"[OTP SERVICE] Invalid OTP for {phoneNumber}. Attempt {attempts}/{MaxAttempts}");
            return false;
        }

        // Success — clean up
        await CleanupKeys(phoneKey);
        Console.WriteLine($"[OTP SERVICE] OTP verified for {phoneNumber}");
        return true;
    }

    private async Task CleanupKeys(string phoneKey)
    {
        await _redis.KeyDeleteAsync($"otp:code:{phoneKey}");
        await _redis.KeyDeleteAsync($"otp:attempts:{phoneKey}");
    }

    private static string GenerateOtp()
    {
#if DEBUG
        return "123456"; // Fixed OTP for development
#else
        // Cryptographically secure random number
        return RandomNumberGenerator.GetInt32(100000, 999999).ToString();
#endif
    }

    private static string HashOtp(string otp)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(otp);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    private static string NormalizePhone(string phone)
    {
        // Remove spaces, dashes — keep only digits and +
        return new string(phone.Where(c => char.IsDigit(c) || c == '+').ToArray());
    }
}

/**
```

---

## What Changed vs Your Old Code

| Before | After |
|--------|-------|
| `ConcurrentDictionary` (dies on restart) | Redis(survives restarts) |
| Plain text OTP stored | SHA256 hashed before storing |
| `Random` for OTP generation | `RandomNumberGenerator` (cryptographically secure) |
| No rate limiting on requests | 3 OTPs per phone per 10 minutes |
| 3 failed attempts, no lockout timer | 3 failed attempts → 15 min lockout |
| No phone normalization | Strips spaces/dashes |

---

## Quick Lesson: Redis Keys We're Using
```
otp:code:+919876543210      → hashed OTP(expires in 5 min)
otp:attempts:+919876543210  → attempt counter(expires in 5 min)
otp:lock:+919876543210      → lockout flag(expires in 15 min)
otp:rate:+919876543210      → request counter(expires in 10 min)
**/