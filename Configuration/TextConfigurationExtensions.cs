using Microsoft.Extensions.Configuration;

namespace CampusRouteLab.Configuration;

public static class TextConfigurationExtensions
{
    public static IConfigurationBuilder AddTextFile(
        this IConfigurationBuilder builder,
        string path)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Путь к customsettings.txt должен быть указан.", nameof(path));
        }

        return builder.Add(new TextConfigurationSource
        {
            Path = path
        });
    }
}
