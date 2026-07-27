using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TailwindIdentity.Core;
using TailwindIdentity.Core.Data;
using TailwindIdentity.Core.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<DefaultContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));

    options.EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
    {

        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();

    }
});

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
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

app.Run();
