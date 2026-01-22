using System.Text.Json.Serialization;

namespace RallyAPI.Integrations.ProRouting.Models;

/// <summary>
/// Response model from ProRouting estimate API.
/// </summary>
internal sealed class ProRoutingEstimateResponse
{
    /// <summary>
    /// Status code. 1 = success.
    /// </summary>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>
    /// Unique estimate ID for this quote.
    /// </summary>
    [JsonPropertyName("estimate_id")]
    public string? EstimateId { get; set; }

    /// <summary>
    /// Estimated delivery time in minutes.
    /// </summary>
    [JsonPropertyName("estimated_delivery_time")]
    public int? EstimatedDeliveryTime { get; set; }

    /// <summary>
    /// Delivery price.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Error message if status != 1
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Checks if the response indicates success.
    /// </summary>
    public bool IsSuccess => Status == 1
                             && !string.IsNullOrEmpty(EstimateId)
                             && Price.HasValue
                             && EstimatedDeliveryTime.HasValue;
}