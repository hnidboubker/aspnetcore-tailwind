namespace TailwindRazorPage.Web.Services;

/// <summary>
/// Strongly-typed SMTP settings bound from the <c>Email</c> section of <c>appsettings.json</c>.
/// </summary>
public class EmailOptions
{
    public const string SectionName = "Email";

    public string From { get; set; } = string.Empty;

    public string SmtpHost { get; set; } = string.Empty;

    public int SmtpPort { get; set; } = 587;

    public string SmtpUser { get; set; } = string.Empty;

    public string SmtpPassword { get; set; } = string.Empty;

    public bool UseSsl { get; set; }
}
