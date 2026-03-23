using System.Net;

namespace StudentPortal.Diagnostics.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Error 404: Resource Not Found. Please check the URL.");
        }
        else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Error 403: Access Denied. Invalid or missing token.");
        }
    }
}