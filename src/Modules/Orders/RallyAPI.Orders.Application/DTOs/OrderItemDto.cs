namespace RallyAPI.Orders.Application.DTOs;

/// <summary>
/// Order item data transfer object.
/// </summary>
public sealed record OrderItemDto
{
    public Guid Id { get; init; }
    public Guid MenuItemId { get; init; }
    public string ItemName { get; init; } = string.Empty;
    public string? ItemDescription { get; init; }
    public string? ImageUrl { get; init; }
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPrice { get; init; }
    public string? SpecialInstructions { get; init; }
}