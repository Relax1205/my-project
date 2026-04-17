using CampusRouteLab.Configuration;
using CampusRouteLab.Extensions;
using CampusRouteLab.Handlers;
using CampusRouteLab.Middleware;

var builder = WebApplication.CreateBuilder(args);

var inMemoryOverrides = new Dictionary<string, string?>
{
    ["Notifications:Sender"] = "config-center-memory@campus.local",
    ["Notifications:Channel"] = "Teams",
    ["FeatureFlags:LiveDiagnostics"] = "true"
};

builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true)
    .AddXmlFile("portal.xml", optional: false, reloadOnChange: true)
    .AddIniFile("notifications.ini", optional: false, reloadOnChange: true)
    .AddTextFile("customsettings.txt")
    .AddInMemoryCollection(inMemoryOverrides)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services.AddConfigCenterServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<PortalHeaderMiddleware>();

app.MapGet("/", EndpointHandlers.GetHome);
app.MapGet("/config/raw", EndpointHandlers.GetRawConfiguration);
app.MapGet("/config/section/portal", EndpointHandlers.GetPortalSection);
app.MapGet("/config/tree", EndpointHandlers.GetConfigurationTree);
app.MapGet("/config/connection", EndpointHandlers.GetConnectionString);
app.MapGet("/config/providers", EndpointHandlers.GetProviders);
app.MapGet("/config/custom", EndpointHandlers.GetCustomConfiguration);
app.MapGet("/config/bind", EndpointHandlers.GetBoundPortalOptions);
app.MapGet("/config/options", EndpointHandlers.GetOptionsSnapshot);
app.MapGet("/config/effective", EndpointHandlers.GetEffectiveConfiguration);

app.Run();
