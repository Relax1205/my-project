using CampusRouteLab.Configuration;
using CampusRouteLab.Models;
using Microsoft.Extensions.Options;

namespace CampusRouteLab.Handlers;

public static class EndpointHandlers
{
    public static IResult GetHome(IWebHostEnvironment environment)
    {
        return Results.Json(new
        {
            Name = "CampusHub.ConfigCenter",
            Description = "Учебный сервис диагностики конфигурации внутреннего портала колледжа.",
            Environment = environment.EnvironmentName,
            Routes = new[]
            {
                "/",
                "/config/raw",
                "/config/section/portal",
                "/config/tree?section=Portal",
                "/config/connection",
                "/config/providers",
                "/config/custom",
                "/config/bind",
                "/config/options",
                "/config/effective"
            }
        });
    }

    public static IResult GetRawConfiguration(
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        return Results.Json(new
        {
            Environment = environment.EnvironmentName,
            Values = new Dictionary<string, string?>
            {
                ["Portal:Title"] = configuration["Portal:Title"],
                ["Portal:Semester"] = configuration["Portal:Semester"],
                ["Portal:SupportEmail"] = configuration["Portal:SupportEmail"],
                ["Notifications:Sender"] = configuration["Notifications:Sender"],
                ["FeatureFlags:EnableDiagnostics"] = configuration["FeatureFlags:EnableDiagnostics"]
            }
        });
    }

    public static IResult GetPortalSection(IConfiguration configuration)
    {
        var portalSection = configuration.GetSection("Portal");
        if (!portalSection.Exists())
        {
            return Results.NotFound(new { Error = "Секция Portal не найдена." });
        }

        var children = portalSection.GetChildren()
            .Select(child => new
            {
                child.Key,
                child.Path,
                child.Value,
                HasChildren = child.GetChildren().Any()
            })
            .ToList();

        return Results.Json(new
        {
            Section = portalSection.Path,
            Exists = portalSection.Exists(),
            Children = children
        });
    }

    public static IResult GetConfigurationTree(IConfiguration configuration, string? section)
    {
        var sectionName = string.IsNullOrWhiteSpace(section) ? "Portal" : section;
        var targetSection = configuration.GetSection(sectionName);

        if (!targetSection.Exists())
        {
            return Results.NotFound(new
            {
                Error = "Секция не найдена.",
                RequestedSection = sectionName
            });
        }

        return Results.Json(new
        {
            RequestedSection = sectionName,
            Tree = BuildSectionTree(targetSection)
        });
    }

    public static IResult GetConnectionString(IConfiguration configuration)
    {
        return Results.Json(new
        {
            Name = "DefaultConnection",
            Value = configuration.GetConnectionString("DefaultConnection")
        });
    }

    public static IResult GetProviders(IConfiguration configuration)
    {
        var root = AsRoot(configuration);
        var providers = root.Providers
            .Select((provider, index) => new
            {
                Order = index + 1,
                ProviderType = provider.GetType().Name,
                Notes = DescribeProvider(provider)
            })
            .ToList();

        return Results.Json(new
        {
            TotalProviders = providers.Count,
            Providers = providers
        });
    }

    public static IResult GetCustomConfiguration(IConfiguration configuration)
    {
        var root = AsRoot(configuration);
        var customSection = configuration.GetSection("CustomText");
        var customValues = customSection.GetChildren()
            .ToDictionary(child => child.Key, child => child.Value);

        return Results.Json(new
        {
            CustomText = customValues,
            PortalSemester = BuildEffectiveValue(root, "Portal:Semester"),
            CustomBanner = BuildEffectiveValue(root, "FeatureFlags:CustomBanner")
        });
    }

    public static IResult GetBoundPortalOptions(IConfiguration configuration)
    {
        var portal = configuration.GetSection("Portal").Get<PortalOptions>() ?? new PortalOptions();

        return Results.Json(new
        {
            BindingMethod = "Get<PortalOptions>()",
            Portal = portal
        });
    }

    public static IResult GetOptionsSnapshot(
        IOptions<PortalOptions> portalOptions,
        IOptions<NotificationOptions> notificationOptions)
    {
        return Results.Json(new
        {
            Portal = portalOptions.Value,
            Notifications = notificationOptions.Value
        });
    }

    public static IResult GetEffectiveConfiguration(
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var root = AsRoot(configuration);

        return Results.Json(new
        {
            Environment = environment.EnvironmentName,
            EffectiveValues = new[]
            {
                BuildEffectiveValue(root, "Portal:Title"),
                BuildEffectiveValue(root, "Portal:SupportEmail"),
                BuildEffectiveValue(root, "Notifications:Sender"),
                BuildEffectiveValue(root, "Portal:Semester")
            }
        });
    }

    private static IConfigurationRoot AsRoot(IConfiguration configuration)
    {
        return configuration as IConfigurationRoot
            ?? throw new InvalidOperationException("Требуется IConfigurationRoot.");
    }

    private static object BuildSectionTree(IConfigurationSection section)
    {
        var children = section.GetChildren().ToList();
        if (children.Count == 0)
        {
            return new
            {
                section.Key,
                section.Path,
                section.Value
            };
        }

        return new
        {
            section.Key,
            section.Path,
            section.Value,
            Children = children.Select(BuildSectionTree).ToList()
        };
    }

    private static object BuildEffectiveValue(IConfigurationRoot root, string key)
    {
        var contributors = root.Providers
            .Select((provider, index) =>
            {
                var hasValue = provider.TryGet(key, out var providerValue);
                return new
                {
                    HasValue = hasValue,
                    Order = index + 1,
                    ProviderType = provider.GetType().Name,
                    Notes = DescribeProvider(provider),
                    Value = providerValue
                };
            })
            .Where(item => item.HasValue)
            .ToList();

        var winner = contributors.LastOrDefault();

        return new
        {
            Key = key,
            FinalValue = root[key],
            WinningProvider = winner?.ProviderType,
            WinningNotes = winner?.Notes,
            Contributors = contributors
        };
    }

    private static string DescribeProvider(IConfigurationProvider provider)
    {
        return provider switch
        {
            TextConfigurationProvider textProvider =>
                $"Custom text provider ({textProvider.SourcePath})",
            _ => provider.ToString() ?? provider.GetType().Name
        };
    }
}
