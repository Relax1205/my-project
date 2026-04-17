namespace CampusRouteLab.Models;

public sealed class PortalOptions
{
    public string Title { get; set; } = string.Empty;

    public string Semester { get; set; } = string.Empty;

    public string SupportEmail { get; set; } = string.Empty;

    public string CampusName { get; set; } = string.Empty;

    public string Dean { get; set; } = string.Empty;

    public string Building { get; set; } = string.Empty;

    public AdminOptions Admin { get; set; } = new();

    public List<string> Modules { get; set; } = new();
}
