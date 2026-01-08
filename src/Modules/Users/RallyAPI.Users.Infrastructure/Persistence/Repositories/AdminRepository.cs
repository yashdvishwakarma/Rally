using Microsoft.EntityFrameworkCore;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.Entities;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Infrastructure.Persistence.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly UsersDbContext _context;

    public AdminRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<Admin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Admin?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Admins
            .AnyAsync(a => a.Email == email, cancellationToken);
    }

    public async Task AddAsync(Admin admin, CancellationToken cancellationToken = default)
    {
        await _context.Admins.AddAsync(admin, cancellationToken);
    }

    public void Update(Admin admin)
    {
        _context.Admins.Update(admin);
    }
}