namespace RallyAPI.Orders.Application.Abstractions;

/// <summary>
/// Unit of Work pattern for transaction management.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}