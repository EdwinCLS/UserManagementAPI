using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenAuthenticationMiddleware> _logger;

    // Token de ejemplo (en producción usarías JWT o validación externa)
    private const string ValidToken = "Bearer my-secret-token";

    public TokenAuthenticationMiddleware(RequestDelegate next, ILogger<TokenAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || authHeader != ValidToken)
        {
            _logger.LogWarning("🔒 Unauthorized access attempt to {Path}", context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
            return;
        }

        await _next(context); // Token válido, continúa con el pipeline
    }
}