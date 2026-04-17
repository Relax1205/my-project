namespace CampusRouteLab.Models;

public sealed class NotificationOptions
{
    public string Sender { get; set; } = string.Empty;

    public string Channel { get; set; } = string.Empty;

    public string Signature { get; set; } = string.Empty;
}
