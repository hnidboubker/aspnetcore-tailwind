//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using MimeKit;

//namespace TailwindIdentity.Core.Services;

//public class MailKitEmailSender : IEmailSender
//{
//    private readonly IConfiguration _config;
//    private readonly ILogger<MailKitEmailSender> _logger;

//    public MailKitEmailSender(IConfiguration config, ILogger<MailKitEmailSender> logger)
//    {
//        _config = config;
//        _logger = logger;
//    }

//    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//    {
//        var message = new MimeMessage();
//        message.From.Add(new MailboxAddress(
//            _config["Email:FromName"] ?? "Tailwind App",
//            _config["Email:From"] ?? "noreply@tailwind.local"));
//        message.To.Add(MailboxAddress.Parse(email));
//        message.Subject = subject;
//        message.Body = new TextPart("html") { Text = htmlMessage };

//        try
//        {
//            using var client = new SmtpClient();
//            var host = _config["Email:SmtpHost"] ?? "localhost";
//            var port = int.Parse(_config["Email:SmtpPort"] ?? "587");
//            var user = _config["Email:SmtpUser"] ?? "";
//            var password = _config["Email:SmtpPassword"] ?? "";

//            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);

//            if (!string.IsNullOrEmpty(user))
//            {
//                await client.AuthenticateAsync(user, password);
//            }

//            await client.SendAsync(message);
//            await client.DisconnectAsync(true);

//            _logger.LogInformation("Email sent to {Email} with subject '{Subject}'", email, subject);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Failed to send email to {Email} with subject '{Subject}'", email, subject);
//        }
//    }
//}
