namespace RallyAPI.Orders.Application.DTOs;

/// <summary>
/// Address data transfer object.
/// </summary>
public sealed record AddressDto
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Pincode { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? Landmark { get; init; }
    public string? BuildingName { get; init; }
    public string? Floor { get; init; }
    public string? ContactPhone { get; init; }
    public string? Instructions { get; init; }
    public string FormattedAddress { get; init; } = string.Empty;
}