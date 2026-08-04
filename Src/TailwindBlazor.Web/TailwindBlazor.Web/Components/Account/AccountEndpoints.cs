using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TailwindIdentity.Core.Models;

namespace TailwindBlazor.Web.Components.Account;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("account");

        group.MapPost("login", async (
            SignInManager<ApplicationUser> signInManager,
            [FromBody] LoginRequest request) =>
        {
            var result = await signInManager.PasswordSignInAsync(
                request.Email, request.Password, request.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return Results.Ok(new { success = true });
            }
            if (result.IsLockedOut)
            {
                return Results.Ok(new { success = false, error = "Compte verrouillé." });
            }

            return Results.Ok(new { success = false, error = "Email ou mot de passe invalide." });
        });

        group.MapPost("register", async (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            [FromBody] RegisterRequest request) =>
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Results.Ok(new { success = true });
            }

            var errors = result.Errors.Select(e => e.Description).ToArray();
            return Results.Ok(new { success = false, errors });
        });

        group.MapPost("logout", async (
            SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok(new { success = true });
        });

        group.MapGet("user", async (
            HttpContext httpContext,
            UserManager<ApplicationUser> userManager) =>
        {
            if (httpContext.User.Identity?.IsAuthenticated != true)
            {
                return Results.Ok(new { authenticated = false });
            }

            var user = await userManager.GetUserAsync(httpContext.User);
            if (user == null)
            {
                return Results.Ok(new { authenticated = false });
            }

            return Results.Ok(new
            {
                authenticated = true,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email
            });
        });

        group.MapPost("profile", async (
            HttpContext httpContext,
            UserManager<ApplicationUser> userManager,
            [FromBody] ProfileRequest request) =>
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user == null)
            {
                return Results.NotFound();
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            await userManager.UpdateAsync(user);

            return Results.Ok(new { success = true });
        });

        group.MapPost("changepassword", async (
            HttpContext httpContext,
            UserManager<ApplicationUser> userManager,
            [FromBody] ChangePasswordRequest request) =>
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user == null)
            {
                return Results.NotFound();
            }

            var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (result.Succeeded)
            {
                return Results.Ok(new { success = true });
            }

            var errors = result.Errors.Select(e => e.Description).ToArray();
            return Results.Ok(new { success = false, errors });
        });
    }
}

public record LoginRequest(string Email, string Password, bool RememberMe);
public record RegisterRequest(string FirstName, string LastName, string Email, string Password);
public record ProfileRequest(string FirstName, string LastName);
public record ChangePasswordRequest(string OldPassword, string NewPassword);
