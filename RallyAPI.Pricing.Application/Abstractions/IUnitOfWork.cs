// RallyAPI.Pricing.Application/Abstractions/IUnitOfWork.cs
namespace RallyAPI.Pricing.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}