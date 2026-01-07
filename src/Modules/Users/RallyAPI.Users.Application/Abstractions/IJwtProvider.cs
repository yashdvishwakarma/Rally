using RallyAPI.Users.Domain.Entities;

namespace RallyAPI.Users.Application.Abstractions;

public interface IJwtProvider
{
    string GenerateCustomerToken(Customer customer);
    string GenerateRiderToken(Rider rider);
    string GenerateRestaurantToken(Restaurant restaurant);
    string GenerateAdminToken(Admin admin);
}