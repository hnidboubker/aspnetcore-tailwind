using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Hasim.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TailwindRazorPage.Web.Services;

namespace TailwindRazorPage.Web.Pages.Account;

public class PasswordForgotModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;

    public PasswordForgotModel(UserManager<AppUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ConfirmationMessage { get; set; }

    public class InputModel
    {
        [Display(Name = "Adresse e-mail")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail est invalide.")]
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        public string Email { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Input.Email);

        // Always show a generic message to avoid user enumeration.
        if (user is not null && await _userManager.IsEmailConfirmedAsync(user))
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var resetLink = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);

            var subject = "Réinitialisation de votre mot de passe";
            var html = $"""
                <h2>Réinitialisation de mot de passe</h2>
                <p>Bonjour,</p>
                <p>Cliquez sur le lien ci-dessous pour réinitialiser votre mot de passe :</p>
                <p><a href="{HtmlEncoder.Default.Encode(resetLink ?? string.Empty)}">Réinitialiser mon mot de passe</a></p>
                <p>Si vous n'êtes pas à l'origine de cette demande, ignorez cet e-mail.</p>
                """;

            await _emailService.SendAsync(Input.Email, subject, html);

            ConfirmationMessage = "Un e-mail de réinitialisation vous a été envoyé.";
        }
        else
        {
            ConfirmationMessage = "Si un compte est associé à cette adresse, un e-mail de réinitialisation sera envoyé.";
        }

        return Page();
    }
}
