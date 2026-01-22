using MediatR;
using Microsoft.Extensions.Logging;
using RallyAPI.Orders.Application.Abstractions;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.Orders.Application.Mappings;
using RallyAPI.Orders.Domain.Abstractions;
using RallyAPI.Orders.Domain.Enums;
using RallyAPI.Orders.Domain.Errors;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Commands.ConfirmOrder;

public sealed class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConfirmOrderCommandHandler> _logger;

    public ConfirmOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ILogger<ConfirmOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<OrderDto>> Handle(ConfirmOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderDto>(OrderErrors.NotFound(command.OrderId));
        }

        // Verify restaurant owns this order
        if (order.RestaurantId != command.RestaurantId)
        {
            return Result.Failure<OrderDto>(OrderErrors.NotRestaurantOrder);
        }

        // Check if already confirmed
        if (order.Status != OrderStatus.Paid)
        {
            return Result.Failure<OrderDto>(
                OrderErrors.InvalidStatusTransition(order.Status.GetDisplayName(), OrderStatus.Confirmed.GetDisplayName()));
        }

        try
        {
            order.Confirm();

            _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order {OrderNumber} confirmed by restaurant {RestaurantId}",
                order.OrderNumber.Value, command.RestaurantId);

            return Result.Success(order.ToDto());
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Failed to confirm order {OrderId}: {Message}", command.OrderId, ex.Message);
            return Result.Failure<OrderDto>(OrderErrors.InvalidStatusTransition(order.Status.GetDisplayName(), "Confirmed"));
        }
    }
}