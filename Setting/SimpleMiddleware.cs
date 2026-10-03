namespace Hirealdoor.Setting;

public class SimpleMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();
        if (ip?.StartsWith("198.") == true)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
        await next(context);
        Console.WriteLine($"Response: {context.Response.StatusCode}");
    }
}