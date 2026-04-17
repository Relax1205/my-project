using CampusRouteLab.Models;
using Microsoft.Extensions.Options;

namespace CampusRouteLab.Middleware;

public sealed class PortalHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<PortalOptions> _options;

    public PortalHeaderMiddleware(RequestDelegate next, IOptions<PortalOptions> options)
    {
        _next = next;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var portal = _options.Value;

        context.Response.Headers["X-Portal-Title"] = portal.Title;
        context.Response.Headers["X-Portal-Semester"] = portal.Semester;

        await _next(context);
    }
}
