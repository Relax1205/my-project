using CampusRouteLab.Services.Interfaces;

namespace CampusRouteLab.Middleware;

public class RequestAuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAppInfoService _appInfo;
    
    public RequestAuditMiddleware(RequestDelegate next, IAppInfoService appInfo)
    {
        _next = next;
        _appInfo = appInfo;
    }
    
    public async Task InvokeAsync(
        HttpContext context,
        IRequestContextService requestContext,
        ITransientMarkerService transientMarker)
    {
        if (context.Request.Path.StartsWithSegments("/diag"))
        {
            Console.WriteLine($"[DIAG BEFORE] RequestId={requestContext.RequestId} | Path={context.Request.Path}");
        }
        
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-App-Instance"] = _appInfo.AppInstanceId.ToString();
            context.Response.Headers["X-Request-Id"] = requestContext.RequestId.ToString();
            context.Response.Headers["X-Transient-Id"] = transientMarker.MarkerId.ToString();
            return Task.CompletedTask;
        });
        
        await _next(context);
        
        if (context.Request.Path.StartsWithSegments("/diag"))
        {
            Console.WriteLine($"[DIAG AFTER] RequestId={requestContext.RequestId} | Status={context.Response.StatusCode}");
        }
    }
}