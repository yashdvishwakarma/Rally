using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Users.Domain.Entities;

public sealed class RefreshToken : BaseEntity
{
    public string TokenHash { get; private set; }     // SHA256 hash of the token
    public Guid UserId { get; private set; }           // Who owns this token
    public string UserType { get; private set; }       // "customer", "rider", "restaurant", "admin"
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public Guid? ReplacedByTokenId { get; private set; } // For rotation chain

    private RefreshToken() { }

    public static RefreshToken Create(
        string tokenHash,
        Guid userId,
        string userType,
        TimeSpan lifetime)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = tokenHash,
            UserId = userId,
            UserType = userType,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(lifetime),
            IsRevoked = false
        };
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;

    public void Revoke(Guid? replacedByTokenId = null)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        ReplacedByTokenId = replacedByTokenId;
    }
}