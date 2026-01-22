using MediatR;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.Orders.Application.Mappings;
using RallyAPI.Orders.Domain.Abstractions;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Queries.GetOrdersByCustomer;

public sealed class GetOrdersByCustomerQueryHandler
    : IRequestHandler<GetOrdersByCustomerQuery, Result<PagedResult<OrderSummaryDto>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByCustomerQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<PagedResult<OrderSummaryDto>>> Handle(
        GetOrdersByCustomerQuery query,
        CancellationToken cancellationToken)
    {
        var skip = (query.Page - 1) * query.PageSize;

        var orders = await _orderRepository.GetByCustomerIdAsync(
            query.CustomerId,
            skip,
            query.PageSize,
            cancellationToken);

        var totalCount = await _orderRepository.GetCountByCustomerIdAsync(
            query.CustomerId,
            cancellationToken);

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