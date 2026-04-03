namespace CampusRouteLab.Services.Interfaces;

public interface IAppInfoService
{
    Guid AppInstanceId { get; }
    DateTime StartedAt { get; }
}