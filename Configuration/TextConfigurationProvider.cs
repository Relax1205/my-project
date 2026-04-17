using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace CampusRouteLab.Configuration;

public sealed class TextConfigurationProvider : ConfigurationProvider
{
    private readonly IFileProvider _fileProvider;

    public TextConfigurationProvider(IFileProvider fileProvider, string sourcePath)
    {
        _fileProvider = fileProvider;
        SourcePath = sourcePath;
    }

    public string SourcePath { get; }

    public override void Load()
    {
        var fileInfo = _fileProvider.GetFileInfo(SourcePath);
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException(
                $"Не найден файл пользовательской конфигурации '{SourcePath}'.",
                SourcePath);
        }

        using var stream = fileInfo.CreateReadStream();
        using var reader = new StreamReader(stream);

        var meaningfulLines = new List<string>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            {
                continue;
            }

            meaningfulLines.Add(line);
        }

        if (meaningfulLines.Count % 2 != 0)
        {
            throw new InvalidDataException(
                $"Файл '{SourcePath}' должен содержать пары строк ключ/значение.");
        }

        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        for (var index = 0; index < meaningfulLines.Count; index += 2)
        {
            var key = meaningfulLines[index];
            var value = meaningfulLines[index + 1];
            data[key] = value;
        }

        Data = data;
    }

    public override string ToString()
    {
        return $"{GetType().Name} for '{SourcePath}'";
    }
}
