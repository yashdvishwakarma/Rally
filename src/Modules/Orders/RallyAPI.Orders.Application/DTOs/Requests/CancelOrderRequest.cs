using RallyAPI.Orders.Domain.Enums;

namespace RallyAPI.Orders.Application.DTOs.Requests;

/// <summary>
/// Request model for cancelling an order.
/// </summary>
public sealed record CancelOrderRequest
{
    public CancellationReason Reason { get; init; }
    public string? Notes { get; init; }
}