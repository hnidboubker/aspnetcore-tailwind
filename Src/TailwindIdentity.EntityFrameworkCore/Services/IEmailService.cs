
namespace TailwindIdentity.EntityFrameworkCore.Services;

/// <summary>
/// Sends emails via MailKit and persists every attempt to the database.
/// </summary>
public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default);
}
