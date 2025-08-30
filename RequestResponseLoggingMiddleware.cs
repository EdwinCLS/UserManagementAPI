using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log request method and path
        var method = context.Request.Method;
        var path = context.Request.Path;
        _logger.LogInformation("➡️ Request: {Method} {Path}", method, path);

        // Hook into response to capture status code
        await _next(context);

        var statusCode = context.Response.StatusCode;
        _logger.LogInformation("⬅️ Response: {StatusCode} for {Method} {Path}", statusCode, method, path);
    }
}