//using Hasim.EntityFrameworkCore.Persistence;
//using Microsoft.EntityFrameworkCore;
//using TailwindRazorPage.Web.Persistence.Models;

//namespace TailwindRazorPage.Web.Persistence;

///// <summary>
///// Application EF Core context. Extends the audit-enabled
///// <see cref="AuditIdentityContext"/> from the Hasim library.
///// </summary>
//public class DefaultContext : AuditIdentityContext
//{
//    public DefaultContext(DbContextOptions<DefaultContext> options)
//        : base(options)
//    {
//    }

//    public DbSet<EmailMessage> EmailMessages => Set<EmailMessage>();
//}
