using StudentPortal.Diagnostics.Extensions;
using StudentPortal.Diagnostics.Middleware;
using StudentPortal.Diagnostics.Services;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStudentPortalServices();
builder.Services.AddSingleton(builder.Environment);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Use(async (context, next) =>
{
    var startTime = DateTime.Now;
    await next();
    var duration = DateTime.Now - startTime;
    
    if (!context.Response.HasStarted)
    {
        context.Response.Headers["X-Processing-Time"] = $"{duration.TotalMilliseconds} ms";
    }
});

app.UseWhen(
    context => context.Request.Query.ContainsKey("trace") && context.Request.Query["trace"] == "true",
    appBranch =>
    {
        appBranch.Use(async (context, next) =>
        {
            context.Response.Headers["X-Debug-Trace"] = "Enabled";
            await next();
        });
    }
);

app.MapWhen(
    context => context.Request.Query.ContainsKey("format") && context.Request.Query["format"] == "plain",
    appBranch =>
    {
        appBranch.Run(async context =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Plain Text Mode: No HTML, no JSON. Just raw data.");
        });
    }
);

app.MapGet("/tools/time", (IDateTimeService timeService) => 
    $"Current Time: {timeService.GetTime()}");

app.MapGet("/tools/date", (IDateTimeService timeService) => 
    $"Current Date: {timeService.GetDate()}");

app.MapGet("/tools/info", (IEnvironmentReportService envService) => 
    $"App Info: {envService.GetAppInfo()}");


app.Map("/secure", secureApp =>
{
    secureApp.UseToken("study2026");
    
    secureApp.Map("/report", reportApp =>
    {
        reportApp.Run(async context =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Secure Report: Access Granted. Confidential Data Here.");
        });
    });
    
    secureApp.Map("/admin/report", adminApp =>
    {
        adminApp.Run(async context =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Admin Secure Report: Top Secret Data.");
        });
    });
});

app.MapGet("/env", (IWebHostEnvironment env) =>
{
    return new
    {
        EnvironmentName = env.EnvironmentName,
        ApplicationName = env.ApplicationName,
        ContentRootPath = env.ContentRootPath,
        WebRootPath = env.WebRootPath,
        IsDevelopment = env.IsDevelopment(),
        IsProduction = env.IsProduction()
    };
});

app.MapGet("/di/services", (ServiceRegistryInfo info) =>
{
    var result = new
    {
        TotalRegisteredServices = info.TotalCount,
        SampleServices = info.ServiceDescriptions.Take(10)
    };
    return result;
});

app.MapGet("/", () => @"
<html>
<head><title>StudentPortal.Diagnostics</title></head>
<body>
    <h1>Welcome to StudentPortal.Diagnostics</h1>
    <p>ASP.NET Core Pipeline Practice (.NET 10)</p>
    <ul>
        <li><a href='/tools/time'>/tools/time</a> - Current Time</li>
        <li><a href='/tools/date'>/tools/date</a> - Current Date</li>
        <li><a href='/tools/info'>/tools/info</a> - App Info</li>
        <li><a href='/tools/time?trace=true'>/tools/time?trace=true</a> - With Trace Header</li>
        <li><a href='/anything?format=plain'>/anything?format=plain</a> - Plain Text Mode</li>
        <li><a href='/secure/report'>/secure/report</a> - Protected (No Token)</li>
        <li><a href='/secure/report?token=study2026'>/secure/report?token=study2026</a> - Protected (Valid Token)</li>
        <li><a href='/env'>/env</a> - Environment Info</li>
        <li><a href='/di/services'>/di/services</a> - DI Services Info</li>
        <li><a href='/unknown'>/unknown</a> - Test 404 Handling</li>
    </ul>
</body>
</html>
");

app.Run();