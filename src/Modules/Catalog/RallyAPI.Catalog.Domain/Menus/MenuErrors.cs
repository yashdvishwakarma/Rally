using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Catalog.Domain.Menus;

public static class MenuErrors 
{
    public static readonly Error NotFound = Error.Create(
        "Menu.NotFound",
        "Menu not found");

    public static readonly Error NameRequired = Error.Create(
        "Menu.NameRequired",
        "Menu name is required");

    public static readonly Error Unauthorized = Error.Create(
        "Menu.Unauthorized",
        "You are not authorized to modify this menu");
}