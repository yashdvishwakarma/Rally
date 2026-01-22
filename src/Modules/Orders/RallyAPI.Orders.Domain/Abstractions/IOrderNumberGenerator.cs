using RallyAPI.Orders.Domain.ValueObjects;

namespace RallyAPI.Orders.Domain.Abstractions;

/// <summary>
/// Generates unique human-readable order numbers.
/// Implementation can use database sequences, distributed IDs, etc.
/// </summary>
public interface IOrderNumberGenerator
{
    Task<OrderNumber> GenerateAsync(CancellationToken cancellationToken = default);
}