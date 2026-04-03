namespace CampusRouteLab.Services.Implementations;

using CampusRouteLab.Services.Interfaces;

public class AppInfoService : IAppInfoService
{
    public Guid AppInstanceId { get; } = Guid.NewGuid();
    public DateTime StartedAt { get; } = DateTime.Now;
}