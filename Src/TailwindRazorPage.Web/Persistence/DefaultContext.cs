using Hasim.EntityFrameworkCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TailwindRazorPage.Web.Persistence;

/// <summary>
/// Application EF Core context. Extends the audit-enabled
/// <see cref="AuditIdentityContext"/> from the Hasim library.
/// </summary>
public class DefaultContext : AuditIdentityContext
{
    public DefaultContext(DbContextOptions<DefaultContext> options)
        : base(options)
    {
    }
}
