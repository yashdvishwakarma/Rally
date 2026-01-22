using System.Text.Json.Serialization;

namespace RallyAPI.Integrations.ProRouting.Models;

/// <summary>
/// Request model for ProRouting estimate API.
/// Maps to POST /partner/estimate
/// </summary>
internal sealed class ProRoutingEstimateRequest
{
    [JsonPropertyName("pickup")]
    public required ProRoutingLocation Pickup { get; set; }

    [JsonPropertyName("drop")]
    public required ProRoutingLocation Drop { get; set; }

    [JsonPropertyName("city")]
    public required string City { get; set; }

    [JsonPropertyName("order_category")]
    public required string OrderCategory { get; set; }

    [JsonPropertyName("search_category")]
    public required string SearchCategory { get; set; }

    [JsonPropertyName("order_amount")]
    public required decimal OrderAmount { get; set; }

    [JsonPropertyName("order_weight")]
    public decimal? OrderWeight { get; set; }
}