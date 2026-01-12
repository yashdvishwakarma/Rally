using RallyAPI.Catalog.Domain.Menus;

namespace RallyAPI.Catalog.Application.Abstractions;

public interface IMenuRepository
{
    Task<Menu?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Menu>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken ct = default);
    void Add(Menu menu);
    void Update(Menu menu);
    void Delete(Menu menu);
}