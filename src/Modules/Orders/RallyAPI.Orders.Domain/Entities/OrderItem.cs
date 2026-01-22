using RallyAPI.Orders.Domain.ValueObjects;
using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Orders.Domain.Entities;

/// <summary>
/// Represents a single item in an order.
/// Denormalized - stores item details at time of order for historical accuracy.
/// </summary>
public sealed class OrderItem : BaseEntity
{
    // Reference to catalog (for linking, not for data)
    public Guid MenuItemId { get; private set; }

    // Denormalized data (captured at order time)
    public string ItemName { get; private set; }
    public string? ItemDescription { get; private set; }
    public string? ImageUrl { get; private set; }

    // Pricing
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public Money TotalPrice { get; private set; }

    // Customizations
    public string? SpecialInstructions { get; private set; }

    // Metadata (flexible JSON for future customizations)
    public string? Metadata { get; private set; }

    // EF Core constructor
    private OrderItem() { }

    private OrderItem(
        Guid menuItemId,
        string itemName,
        string? itemDescription,
        string? imageUrl,
        Money unitPrice,
        int quantity,
        string? specialInstructions,
        string? metadata)
    {
        Id = Guid.NewGuid();
        MenuItemId = menuItemId;
        ItemName = itemName;
        ItemDescription = itemDescription;
        ImageUrl = imageUrl;
        UnitPrice = unitPrice;
        Quantity = quantity;
        TotalPrice = unitPrice.Multiply(quantity);
        SpecialInstructions = specialInstructions;
        Metadata = metadata;
    }

    public static OrderItem Create(
        Guid menuItemId,
        string itemName,
        Money unitPrice,
        int quantity,
        string? itemDescription = null,
        string? imageUrl = null,
        string? specialInstructions = null,
        string? metadata = null)
    {
        if (menuItemId == Guid.Empty)
            throw new ArgumentException("Menu item ID is required", nameof(menuItemId));

        if (string.IsNullOrWhiteSpace(itemName))
            throw new ArgumentException("Item name is required", nameof(itemName));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        return new OrderItem(
            menuItemId,
            itemName.Trim(),
            itemDescription?.Trim(),
            imageUrl?.Trim(),
            unitPrice,
            quantity,
            specialInstructions?.Trim(),
            metadata);
    }

    /// <summary>
    /// Updates quantity and recalculates total
    /// </summary>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));

        Quantity = newQuantity;
        TotalPrice = UnitPrice.Multiply(newQuantity);
    }

    /// <summary>
    /// Updates special instructions
    /// </summary>
    public void UpdateInstructions(string? instructions)
    {
        SpecialInstructions = instructions?.Trim();
    }
}