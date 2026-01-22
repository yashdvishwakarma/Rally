using MediatR;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.Orders.Application.Mappings;
using RallyAPI.Orders.Domain.Abstractions;
using RallyAPI.Orders.Domain.Enums;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Queries.GetActiveOrders;

public sealed class GetActiveOrdersQueryHandler
    : IRequestHandler<GetActiveOrdersQuery, Result<IReadOnlyList<OrderSummaryDto>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetActiveOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<IReadOnlyList<OrderSummaryDto>>> Handle(
        GetActiveOrdersQuery query,
        CancellationToken cancellationToken)
    {
        // Get orders in active statuses
        var activeStatuses = new[]
        {
            OrderStatus.Paid,   
            OrderStatus.Confirmed,
            OrderStatus.Preparing,
            OrderStatus.ReadyForPickup,
            OrderStatus.PickedUp
        };

        var allActiveOrders = new List<Domain.Entities.Order>();

        foreach (var status in activeStatuses)
        {
            var orders = await _orderRepository.GetByStatusAsync(status, 0, query.Limit, cancellationToken);
            allActiveOrders.AddRange(orders);
        }

        var result = allActiveOrders
            .OrderByDescending(o => o.CreatedAt)
            .Take(query.Limit)
            .Select(o => o.ToSummaryDto())
            .ToList();

        return Result.Success<IReadOnlyList<OrderSummaryDto>>(result);
    }
}