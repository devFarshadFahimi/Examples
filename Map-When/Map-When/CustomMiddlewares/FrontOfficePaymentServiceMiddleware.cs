using System.Text.Json;

namespace CustomMiddlewares;

public class FrontOfficePaymentServiceMiddleware
{
    private readonly RequestDelegate _next;

    public FrontOfficePaymentServiceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (DateTime.Now.Hour > 20)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status451UnavailableForLegalReasons;

            var data = new { Message = "Payment services are not available after 9:00 p.m." };

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
