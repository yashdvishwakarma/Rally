using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.Entities;

namespace RallyAPI.Users.Infrastructure.Services;

public class JwtProvider : IJwtProvider
{
    private readonly JwtSettings _settings;

    public JwtProvider(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateCustomerToken(Customer customer)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("phone", customer.Phone.Value),
            new("role", "Customer"),
            new("user_type", "customer")
        };

        if (!string.IsNullOrEmpty(customer.Name))
            claims.Add(new Claim("name", customer.Name));

        return GenerateToken(claims);
    }

    public string GenerateRiderToken(Rider rider)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, rider.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("phone", rider.Phone.Value),
            new("name", rider.Name),
            new("role", "Rider"),
            new("user_type", "rider"),
            new("kyc_status", rider.KycStatus.ToString())
        };

        return GenerateToken(claims);
    }

    public string GenerateRestaurantToken(Restaurant restaurant)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, restaurant.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, restaurant.Email.Value),
            new("name", restaurant.Name),
            new("role", "Restaurant"),
            new("user_type", "restaurant")
        };

        return GenerateToken(claims);
    }

    public string GenerateAdminToken(Admin admin)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, admin.Email.Value),
            new("name", admin.Name),
            new("role", admin.Role.ToString()),
            new("user_type", "admin")
        };

        return GenerateToken(claims);
    }

    private string GenerateToken(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}