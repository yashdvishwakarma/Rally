using MediatR;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.Orders.Application.Mappings;
using RallyAPI.Orders.Application.Queries.GetOrdersByCustomer;
using RallyAPI.Orders.Domain.Abstractions;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Queries.GetOrdersByRestaurant;

public sealed class GetOrdersByRestaurantQueryHandler
    : IRequestHandler<GetOrdersByRestaurantQuery, Result<PagedResult<OrderSummaryDto>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByRestaurantQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<PagedResult<OrderSummaryDto>>> Handle(
        GetOrdersByRestaurantQuery query,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Entities.Order> orders;
        int totalCount;

        if (query.ActiveOnly)
        {
            orders = await _orderRepository.GetActiveOrdersByRestaurantAsync(
                query.RestaurantId,
                cancellationToken);
            totalCount = orders.Count;
        }
        else
        {
            var skip = (query.Page - 1) * query.PageSize;
            orders = await _orderRepository.GetByRestaurantIdAsync(
                query.RestaurantId,
                skip,
                query.PageSize,
                cancellationToken);
            totalCount = await _orderRepository.GetCountByRestaurantIdAsync(
                query.RestaurantId,
                cancellationToken);
        }

        var result = new PagedResult<OrderSummaryDto>
        {
            Items = orders.Select(o => o.ToSummaryDto()).ToList(),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };

        return Result.Success(result);
    }
}