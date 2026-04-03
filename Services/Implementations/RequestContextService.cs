namespace CampusRouteLab.Services.Implementations;

using CampusRouteLab.Services.Interfaces;

public class RequestContextService : IRequestContextService
{
    public Guid RequestId { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.Now;
}