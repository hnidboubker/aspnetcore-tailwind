using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using TailwindIdentity.Core.Enums;
using TailwindIdentity.Core.Models;
using TailwindIdentity.EntityFrameworkCore.Persistence;

namespace TailwindIdentity.EntityFrameworkCore.Services;

/// <summary>
/// Sends emails through MailKit and persists every attempt (sent or failed)
/// to the <see cref="EmailMessage"/> table for audit purposes.
/// </summary>
public class MailKitEmailSender : IEmailService, IEmailSender
{
    private readonly DefaultContext _db;
    private readonly IOptions<EmailOptions> _options;

    public MailKitEmailSender(DefaultContext db, IOptions<EmailOptions> options)
    {
        _db = db;
        _options = options;
    }

    /// <summary>
    /// Identity UI compatibility overload.
    /// </summary>
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return SendAsync(email, subject, htmlMessage, isHtml: true);
    }

    public async Task SendAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default)
    {
        var record = new EmailMessage
        {
            From = _options.Value.From,
            To = to,
            Subject = subject,
            Body = body,
            IsHtml = isHtml,
            Status = EmailStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _db.EmailMessages.Add(record);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_options.Value.From));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder();
            if (isHtml)
            {
                builder.HtmlBody = body;
            }
            else
            {
                builder.TextBody = body;
            }
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _options.Value.SmtpHost,
                _options.Value.SmtpPort,
                _options.Value.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable,
                cancellationToken);

            if (!string.IsNullOrEmpty(_options.Value.SmtpUser))
            {
                await client.AuthenticateAsync(_options.Value.SmtpUser, _options.Value.SmtpPassword, cancellationToken);
            }

            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            record.Status = EmailStatus.Sent;
            record.SentAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            record.Status = EmailStatus.Failed;
            record.ErrorMessage = ex.Message;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
