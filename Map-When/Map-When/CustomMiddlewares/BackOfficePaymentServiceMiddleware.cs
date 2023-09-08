using System.Text.Json;

namespace CustomMiddlewares;

public class BackOfficePaymentServiceMiddleware
{
    private readonly RequestDelegate _next;

    public BackOfficePaymentServiceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("PaymentServiceKey"))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var data = new { Message = "Invalid Payment Key!" };

            var json = JsonSerializer.Serialize(data);

            await context.Response.WriteAsync(json);
        }
        else
        {
            // some related business logic.
            await _next(context);
        }
    }
}