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

        group.MapGet("confirm-email", async (
            UserManager<ApplicationUser> userManager,
            [FromQuery] string? userId,
            [FromQuery] string? code) =>
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return Results.BadRequest(new { error = "Paramètres manquants." });
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Results.NotFound(new { error = "Utilisateur introuvable." });
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            return Results.Ok(new { success = result.Succeeded });
        });

        group.MapPost("forgot-password", async (
            HttpContext httpContext,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            [FromBody] ForgotPasswordRequest request) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/reset-password?code={Uri.EscapeDataString(code)}";

                await emailSender.SendEmailAsync(
                    request.Email,
                    "Réinitialisation de mot de passe",
                    $"<p>Cliquez sur <a href='{callbackUrl}'>ce lien</a> pour réinitialiser votre mot de passe.</p>");
            }

            // Toujours répondre de la même manière (anti-énumération de comptes)
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

public record ForgotPasswordRequest(string Email);
public record LoginRequest(string Email, string Password, bool RememberMe);
public record RegisterRequest(string FirstName, string LastName, string Email, string Password);
public record ProfileRequest(string FirstName, string LastName);
public record ChangePasswordRequest(string OldPassword, string NewPassword);
