using RallyAPI.Orders.Application.Abstractions;

namespace RallyAPI.Orders.Infrastructure;

/// <summary>
/// Unit of Work implementation for Orders module.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _context;

    public UnitOfWork(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}