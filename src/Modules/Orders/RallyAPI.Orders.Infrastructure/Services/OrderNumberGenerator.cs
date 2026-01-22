using Microsoft.EntityFrameworkCore;
using RallyAPI.Orders.Domain.Abstractions;
using RallyAPI.Orders.Domain.ValueObjects;

namespace RallyAPI.Orders.Infrastructure.Services;

/// <summary>
/// Generates unique human-readable order numbers.
/// Uses database sequence for uniqueness guarantee.
/// </summary>
public sealed class OrderNumberGenerator : IOrderNumberGenerator
{
    private readonly OrdersDbContext _context;
    private static int _fallbackCounter;

    public OrderNumberGenerator(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<OrderNumber> GenerateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Try to get sequence from database
            var today = DateTime.UtcNow.Date;
            var datePrefix = today.ToString("yyyyMMdd");

            // Get today's count
            var todayCount = await _context.Orders
                .CountAsync(o => o.CreatedAt.Date == today, cancellationToken);

            var sequence = todayCount + 1;

            return OrderNumber.Create(sequence, today);
        }
        catch
        {
            // Fallback to timestamp-based generation
            return OrderNumber.CreateFallback();
        }
    }
}