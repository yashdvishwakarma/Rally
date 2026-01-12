using Microsoft.EntityFrameworkCore;
using RallyAPI.Catalog.Application.Abstractions;
using RallyAPI.Catalog.Domain.Menus;

namespace RallyAPI.Catalog.Infrastructure.Persistence.Repositories;

internal sealed class MenuRepository : IMenuRepository
{
    private readonly CatalogDbContext _context;

    public MenuRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Menu?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Menus.FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<List<Menu>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken ct = default)
    {
        return await _context.Menus
            .Where(m => m.RestaurantId == restaurantId && m.IsActive)
            .OrderBy(m => m.DisplayOrder)
            .ToListAsync(ct);
    }

    public void Add(Menu menu) => _context.Menus.Add(menu);

    public void Update(Menu menu) => _context.Menus.Update(menu);

    public void Delete(Menu menu) => _context.Menus.Remove(menu);
}