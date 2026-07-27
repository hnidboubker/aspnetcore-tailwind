using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TailwindIdentity.Core.Models;


namespace TailwindIdentity.Core.Data;

public class DefaultContext : IdentityDbContext<ApplicationUser, 
                                                ApplicationRole, int, 
                                                ApplicationUserClaim,
                                                ApplicationUserRole, 
                                                ApplicationUserLogin,
                                                ApplicationRoleClaim,
                                                 ApplicationUserToken>
{
    public DefaultContext(DbContextOptions<DefaultContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("User");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        builder.Entity<ApplicationRole>(entity =>
        {
           entity.ToTable("Role"); 
        });

        builder.Entity<ApplicationUserRole>(entity =>
        {
           entity.ToTable("UserRole"); 
        });

        builder.Entity<ApplicationUserClaim>(entity =>
        {
           entity.ToTable("UserClaim"); 
        });

        builder.Entity<ApplicationUserToken>(entity =>
        {
            entity.ToTable("UserToken");
        });

        builder.Entity<ApplicationRoleClaim>(entity =>
        {
           entity.ToTable("RoleClaim"); 
        });

        builder.Entity<ApplicationUserLogin>(entity =>
        {
            
        });
    }
}
