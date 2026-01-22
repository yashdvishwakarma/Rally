namespace RallyAPI.Orders.Application.DTOs.Requests;

/// <summary>
/// Request model for assigning a rider to an order.
/// </summary>
public sealed record AssignRiderRequest
{
    public Guid RiderId { get; init; }
    public string? RiderName { get; init; }
    public string? RiderPhone { get; init; }
}