using CampusRouteLab.Models;

namespace CampusRouteLab.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigCenterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PortalOptions>(configuration.GetSection("Portal"));
        services.Configure<NotificationOptions>(configuration.GetSection("Notifications"));

        return services;
    }
}
