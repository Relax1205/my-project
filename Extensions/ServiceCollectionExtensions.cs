using StudentPortal.Diagnostics.Services;
using Microsoft.Extensions.DependencyInjection;

namespace StudentPortal.Diagnostics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStudentPortalServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<IEnvironmentReportService, EnvironmentReportService>();
        
        var registryInfo = new ServiceRegistryInfo();
        services.AddSingleton(registryInfo);

        registryInfo.TotalCount = services.Count; 
        registryInfo.ServiceDescriptions.Add("IDateTimeService -> DateTimeService (Singleton)");
        registryInfo.ServiceDescriptions.Add("IEnvironmentReportService -> EnvironmentReportService (Singleton)");
        registryInfo.ServiceDescriptions.Add("ServiceRegistryInfo (Singleton)");
        registryInfo.ServiceDescriptions.Add("IWebHostEnvironment (Built-in)");
        registryInfo.ServiceDescriptions.Add("ILogger<Program> (Built-in)");
        registryInfo.ServiceDescriptions.Add("IHttpContextAccessor (Built-in)");
        registryInfo.ServiceDescriptions.Add("OptionsMonitor<...> (Built-in)");
        registryInfo.ServiceDescriptions.Add("HttpClient (Built-in)");
        registryInfo.ServiceDescriptions.Add("AuthorizationService (Built-in)");
        registryInfo.ServiceDescriptions.Add("AuthenticationService (Built-in)");

        return services;
    }
}