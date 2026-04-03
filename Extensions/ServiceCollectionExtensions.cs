namespace CampusRouteLab.Extensions;

using Microsoft.Extensions.DependencyInjection;
using CampusRouteLab.Services.Interfaces;
using CampusRouteLab.Services.Implementations;
using CampusRouteLab.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCampusServices(this IServiceCollection services)
    {
        services.AddSingleton<IAppInfoService, AppInfoService>();
        services.AddSingleton<IStudentCatalogService, StudentCatalogService>();
        
        services.AddScoped<IRequestContextService, RequestContextService>();
        
        services.AddTransient<ITransientMarkerService, TransientMarkerService>();
        services.AddTransient<DiagnosticsReportService>();
        
        return services;
    }
}