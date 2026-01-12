using RallyAPI.Catalog.Application.Abstractions;

namespace RallyAPI.Catalog.Infrastructure.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly CatalogDbContext _context;

    public UnitOfWork(CatalogDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}