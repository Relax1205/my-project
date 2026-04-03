using CampusRouteLab.Extensions;
using CampusRouteLab.Middleware;
using CampusRouteLab.Handlers;
using CampusRouteLab.Services.Interfaces;
using CampusRouteLab.Services;
using Microsoft.AspNetCore.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCampusServices();

var app = builder.Build();

app.UseRequestAudit();

app.MapGet("/", () => Results.Json(new
{
    Name = "CampusRouteLab",
    Description = "Diagnostic service for routing and DI demonstration",
    Version = "1.0.0",
    Endpoints = new[] 
    { 
        "/students", 
        "/reports", 
        "/portal", 
        "/files",
        "/routes",
        "/diag/lifetimes"
    }
}))
.WithName("Root");

app.MapGet("/students", EndpointHandlers.GetStudentsList)
    .WithName("GetStudentsList");

app.MapGet("/students/{group}", (string group, IStudentCatalogService catalog) =>
{
    var data = catalog.GetGroupByName(group);
    return data is not null 
        ? Results.Json(data) 
        : Results.NotFound(new { Error = "Group not found", Group = group });
})
.WithName("GetStudentByGroup");

app.MapGet("/students/{group}/{id}", EndpointHandlers.GetStudentById)
    .WithName("GetStudentById");

app.MapGet("/reports/{section?}", (string? section) =>
{
    var sec = string.IsNullOrEmpty(section) ? "overview" : section;
    return Results.Json(new 
    { 
        Section = sec, 
        Content = $"Report for {sec}",
        ParameterUsed = !string.IsNullOrEmpty(section)
    });
})
.WithName("GetReport");

app.MapGet("/portal/{module=home}/{page=index}/{id?}", 
    (string module, string page, int? id) =>
{
    return Results.Json(new
    {
        Module = module,
        Page = page,
        Id = id,
        DefaultsUsed = new
        {
            ModuleDefault = module == "home",
            PageDefault = page == "index",
            IdDefault = id == null
        }
    });
})
.WithName("GetPortal");

app.MapGet("/files/{**path}", (string path) =>
    Results.Text($"Catch-all captured: {path}"))
    .WithName("GetFiles");

app.MapGet("/routes", (EndpointDataSource dataSource) =>
{
    var routes = dataSource.Endpoints
        .OfType<RouteEndpoint>()
        .Select(e => new 
        { 
            Route = e.RoutePattern.RawText,
            DisplayName = e.DisplayName ?? "Unnamed"
        })
        .OrderBy(r => r.Route)
        .ToList();
    
    return Results.Json(new 
    { 
        TotalEndpoints = routes.Count, 
        Routes = routes 
    });
})
.WithName("GetRoutes");

app.MapGet("/diag/lifetimes", (
    IAppInfoService appInfo,
    IRequestContextService reqContext,
    ITransientMarkerService transient,
    DiagnosticsReportService reportService) =>
{
    var direct = new
    {
        Singleton = appInfo.AppInstanceId,
        Scoped = reqContext.RequestId,
        Transient = transient.MarkerId,
        Source = "Handler"
    };
    
    var fromService = reportService.GetLifetimeReport();
    
    return Results.Json(new 
    { 
        Direct = direct, 
        FromService = fromService,
        Comparison = new
        {
            SingletonMatch = direct.Singleton == fromService.SingletonAppId,
            ScopedMatch = direct.Scoped == fromService.ScopedRequestId,
            TransientMatch = direct.Transient == fromService.TransientMarkerId
        }
    });
})
.WithName("GetLifetimes");

app.MapGet("/diag/lifetimes/check", (IServiceProvider sp) =>
{
    var t1 = sp.GetRequiredService<ITransientMarkerService>();
    var t2 = sp.GetRequiredService<ITransientMarkerService>();
    
    return Results.Json(new
    {
        First = t1.MarkerId,
        Second = t2.MarkerId,
        AreDifferent = t1.MarkerId != t2.MarkerId,
        Explanation = "Transient services are created new each time"
    });
})
.WithName("CheckLifetimes");

app.MapGet("/diag/request-services", (HttpContext context) =>
{
    var svc = context.RequestServices.GetRequiredService<IRequestContextService>();
    return Results.Json(new 
    { 
        RequestId = svc.RequestId,
        Method = "HttpContext.RequestServices.GetRequiredService"
    });
})
.WithName("GetRequestServices");

app.MapGet("/diag/app-services", () =>
{
    var svc = app.Services.GetRequiredService<IAppInfoService>();
    return Results.Json(new 
    { 
        AppInstanceId = svc.AppInstanceId, 
        StartedAt = svc.StartedAt,
        Method = "app.Services.GetRequiredService"
    });
})
.WithName("GetAppServices");

app.Run();