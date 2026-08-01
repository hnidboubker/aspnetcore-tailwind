namespace TailwindRazorPage.Web.Persistence.Models;

/// <summary>
/// Status of an email send attempt.
/// </summary>
public enum EmailStatus
{
    Pending = 0,
    Sent = 1,
    Failed = 2
}

/// <summary>
/// Represents an email that was (or attempted to be) sent, persisted for audit purposes.
/// </summary>
public class EmailMessage
{
    public int Id { get; set; }

    public string From { get; set; } = string.Empty;

    public string To { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string? Body { get; set; }

    public bool IsHtml { get; set; }

    public EmailStatus Status { get; set; }

    public DateTime? SentAt { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
