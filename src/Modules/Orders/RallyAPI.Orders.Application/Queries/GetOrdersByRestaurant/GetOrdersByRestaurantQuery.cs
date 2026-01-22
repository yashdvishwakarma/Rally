using MediatR;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.Orders.Application.Queries.GetOrdersByCustomer;
using RallyAPI.Orders.Domain.Enums;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Queries.GetOrdersByRestaurant;

/// <summary>
/// Query to get orders by restaurant ID with optional filtering.
/// </summary>
public sealed record GetOrdersByRestaurantQuery : IRequest<Result<PagedResult<OrderSummaryDto>>>
{
    public Guid RestaurantId { get; init; }
    public OrderStatus? Status { get; init; }
    public bool ActiveOnly { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}