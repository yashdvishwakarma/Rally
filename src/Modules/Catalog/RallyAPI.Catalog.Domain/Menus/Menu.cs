using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Catalog.Domain.Menus;

public class Menu : AggregateRoot
{
    public Guid RestaurantId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Menu() { } // EF Core

    public static Menu Create(
        Guid restaurantId,
        string name,
        string? description,
        int displayOrder)
    {
        var menu = new Menu
        {
            Id = Guid.NewGuid(),
            RestaurantId = restaurantId,
            Name = name,
            Description = description,
            DisplayOrder = displayOrder,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return menu;
    }

    public void Update(string name, string? description, int displayOrder)
    {
        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}