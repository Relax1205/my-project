namespace CampusRouteLab.Services.Interfaces;

public interface IRequestContextService
{
    Guid RequestId { get; }
    DateTime CreatedAt { get; }
}