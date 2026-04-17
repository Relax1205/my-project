using Microsoft.Extensions.Configuration;

namespace CampusRouteLab.Configuration;

public sealed class TextConfigurationSource : IConfigurationSource
{
    public string Path { get; set; } = string.Empty;

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new TextConfigurationProvider(builder.GetFileProvider(), Path);
    }
}
