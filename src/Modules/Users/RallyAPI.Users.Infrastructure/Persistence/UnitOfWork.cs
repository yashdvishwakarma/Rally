using RallyAPI.Users.Application.Abstractions;

namespace RallyAPI.Users.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly UsersDbContext _context;

    public UnitOfWork(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}