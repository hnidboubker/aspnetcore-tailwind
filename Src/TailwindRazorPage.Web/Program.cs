using Hasim.Core.Entities;
using Injectify.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TailwindRazorPage.Web.Data;
using TailwindRazorPage.Web.Persistence;
using TailwindRazorPage.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// SMTP email configuration (section "Email").
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.SectionName));
builder.Services.AddScoped<IEmailService, MailKitEmailSender>();
builder.Services.AddScoped<IEmailSender, MailKitEmailSender>();

// Hasim EF Core module: registers AuditIdentityContext (via DefaultContext)
// with SQL Server + DefaultConnection + audit interceptor.
builder.Services.AddDbContext<DefaultContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

// Register the Injectify modules discovered in the loaded assemblies.
builder.InjectifyApplication();

builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail =
            builder.Configuration.GetValue<bool>("Identity:RequireConfirmedEmail");

        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<DefaultContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.InjectifyInitializer();

app.Run();
