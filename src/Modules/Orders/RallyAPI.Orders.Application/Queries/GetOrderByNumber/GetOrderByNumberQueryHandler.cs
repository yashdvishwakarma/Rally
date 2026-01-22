using MediatR;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.Orders.Application.Mappings;
using RallyAPI.Orders.Domain.Abstractions;
using RallyAPI.Orders.Domain.Errors;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Queries.GetOrderByNumber;

public sealed class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQuery, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByNumberQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByNumberQuery query, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(query.OrderNumber, cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderDto>(OrderErrors.NotFoundByNumber(query.OrderNumber));
        }

        return Result.Success(order.ToDto());
    }
}