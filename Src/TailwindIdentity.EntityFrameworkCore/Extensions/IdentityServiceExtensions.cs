using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TailwindIdentity.Core.Models;
using TailwindIdentity.EntityFrameworkCore.Persistence;
using TailwindIdentity.EntityFrameworkCore.Services;

namespace TailwindIdentity.EntityFrameworkCore.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddTailwindIdentity( this IServiceCollection services,    IConfiguration configuration)
    
       
    {
        services.AddDbContext<DefaultContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = configuration.GetValue<bool>("Identity:RequireConfirmedEmail");
        })
        .AddEntityFrameworkStores<DefaultContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/SignIn";
            options.LogoutPath = "/Account/SignOut";
            options.AccessDeniedPath = "/Account/SignIn";
        });

        //services.AddSingleton<IEmailSender, MailKitEmailSender>();

       services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
        services.AddScoped<IEmailService, MailKitEmailSender>();
        services.AddScoped<IEmailSender, MailKitEmailSender>();


        return services;
    }
}
