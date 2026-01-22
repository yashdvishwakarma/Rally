using MediatR;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Queries.GetActiveOrders;

/// <summary>
/// Query to get all active orders (for admin/dashboard).
/// </summary>
public sealed record GetActiveOrdersQuery : IRequest<Result<IReadOnlyList<OrderSummaryDto>>>
{
    public int Limit { get; init; } = 50;
}