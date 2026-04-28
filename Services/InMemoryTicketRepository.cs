using HelpDesk.Results.Models;

namespace HelpDesk.Results.Services;

public sealed class InMemoryTicketRepository : ITicketRepository
{
    private readonly object _syncRoot = new();
    private readonly ILogger<InMemoryTicketRepository> _logger;
    private readonly List<Ticket> _tickets =
    [
        new(1, "Reset student portal password", "Open", 2, DateTime.UtcNow.AddHours(-8)),
        new(2, "Fix projector in room 214", "In Progress", 3, DateTime.UtcNow.AddHours(-4)),
        new(3, "Prepare VPN access instruction", "Resolved", 1, DateTime.UtcNow.AddDays(-1))
    ];

    private int _nextId = 4;

    public InMemoryTicketRepository(ILogger<InMemoryTicketRepository> logger)
    {
        _logger = logger;
    }

    public IEnumerable<Ticket> GetAll()
    {
        lock (_syncRoot)
        {
            return _tickets
                .OrderBy(ticket => ticket.Id)
                .ToArray();
        }
    }

    public Ticket? GetById(int id)
    {
        lock (_syncRoot)
        {
            var ticket = _tickets.FirstOrDefault(ticket => ticket.Id == id);
            _logger.LogInformation("Ticket lookup for id {TicketId}: {Found}", id, ticket is not null);
            return ticket;
        }
    }

    public Ticket Create(string title, int priority)
    {
        lock (_syncRoot)
        {
            var ticket = new Ticket(
                _nextId++,
                title.Trim(),
                "Open",
                priority,
                DateTime.UtcNow);

            _tickets.Add(ticket);
            _logger.LogInformation("Created ticket {TicketId} with priority {Priority}", ticket.Id, priority);

            return ticket;
        }
    }
}
