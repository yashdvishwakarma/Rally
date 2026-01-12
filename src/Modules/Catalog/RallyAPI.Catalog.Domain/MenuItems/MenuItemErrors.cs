using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Domain.MenuItems;

public static class MenuItemErrors 
{
    public static readonly Error NotFound = Error.Create(
        "MenuItem.NotFound",
        "Menu item not found");

    public static readonly Error NameRequired = Error.Create(
        "MenuItem.NameRequired",
        "Menu item name is required");

    public static readonly Error InvalidPrice = Error.Create(
        "MenuItem.InvalidPrice",
        "Price must be greater than zero");

    public static readonly Error Unauthorized = Error.Create(
        "MenuItem.Unauthorized",
        "You are not authorized to modify this item");

    public static readonly Error MenuNotFound = Error.Create(
        "MenuItem.MenuNotFound",
        "The specified menu does not exist");
}