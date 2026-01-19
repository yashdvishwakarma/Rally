using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Integrations.ProRouting;

/// <summary>
/// ProRouting-specific error definitions.
/// </summary>
public static class ProRoutingErrors
{
    public static readonly Error Disabled = Error.Create(
        "ProRouting.Disabled",
        "ProRouting integration is disabled");

    public static readonly Error EmptyResponse = Error.Create(
        "ProRouting.EmptyResponse",
        "Empty response received from ProRouting API");

    public static readonly Error InvalidResponse = Error.Create(
        "ProRouting.InvalidResponse",
        "Invalid response format from ProRouting API");

    public static readonly Error Timeout = Error.Create(
        "ProRouting.Timeout",
        "ProRouting API request timed out");

    public static readonly Error NetworkError = Error.Create(
        "ProRouting.NetworkError",
        "Network error while calling ProRouting API");

    public static Error ApiError(string message) => Error.Create(
        "ProRouting.ApiError",
        message);

    public static Error HttpError(int statusCode, string? reason) => Error.Create(
        "ProRouting.HttpError",
        $"API returned {statusCode}: {reason ?? "Unknown error"}");

    public static Error QuoteFailed(string message) => Error.Create(
        "ProRouting.QuoteFailed",
        message);
}