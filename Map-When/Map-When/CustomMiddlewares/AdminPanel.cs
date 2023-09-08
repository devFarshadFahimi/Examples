using System.Text.Json;

namespace CustomMiddlewares;
public class AdminMiddleware
{
    private readonly RequestDelegate _next;

    public AdminMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("AdminApiKey"))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var data = new { Message = "Invalid Key!" };

            var json = JsonSerializer.Serialize(data);

            await context.Response.WriteAsync(json);
        }
        else
        {
            await _next(context);
        }
    }
}
