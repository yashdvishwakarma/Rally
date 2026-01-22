using MediatR;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Commands.ConfirmOrder;

/// <summary>
/// Command to confirm an order (restaurant accepts).
/// </summary>
public sealed record ConfirmOrderCommand(Guid OrderId, Guid RestaurantId) : IRequest<Result<OrderDto>>;