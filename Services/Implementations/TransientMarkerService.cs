namespace CampusRouteLab.Services.Implementations;

using CampusRouteLab.Services.Interfaces;

public class TransientMarkerService : ITransientMarkerService
{
    public Guid MarkerId { get; } = Guid.NewGuid();
}