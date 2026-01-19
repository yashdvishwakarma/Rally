using Microsoft.AspNetCore.Builder;
using RallyAPI.SharedKernel.Middleware;

namespace RallyAPI.SharedKernel.Extensions;

/// <summary>
/// Extension methods for registering SharedKernel middleware.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds global exception handling middleware to the pipeline.
    /// Should be registered early in the pipeline to catch all exceptions.
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}