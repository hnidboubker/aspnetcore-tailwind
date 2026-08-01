using Hasim.Core.Entities;
using Injectify.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TailwindRazorPage.Web.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

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
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<DefaultContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

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
