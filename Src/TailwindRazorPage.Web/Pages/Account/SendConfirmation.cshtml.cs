using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using TailwindIdentity.Core.Models;
using TailwindIdentity.EntityFrameworkCore.Services;

namespace TailwindRazorPage.Web.Pages.Account;

public class SendConfirmationModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public SendConfirmationModel(UserManager<ApplicationUser> userManager, IEmailService emailService)
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
        if (user is not null && !await _userManager.IsEmailConfirmedAsync(user))
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var confirmLink = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code },
                protocol: Request.Scheme);

            var subject = "Confirmez votre adresse e-mail";
            var html = $"""
                <h2>Confirmation d'adresse e-mail</h2>
                <p>Bonjour,</p>
                <p>Merci de confirmer votre adresse e-mail en cliquant sur le lien ci-dessous :</p>
                <p><a href="{HtmlEncoder.Default.Encode(confirmLink ?? string.Empty)}">Confirmer mon adresse e-mail</a></p>
                <p>Si vous n'êtes pas à l'origine de cette demande, ignorez cet e-mail.</p>
                """;

            await _emailService.SendAsync(Input.Email, subject, html);

            ConfirmationMessage = "Un e-mail de confirmation vous a été envoyé.";
        }
        else
        {
            ConfirmationMessage = "Si un compte est associé à cette adresse, un e-mail de confirmation sera envoyé.";
        }

        return Page();
    }
}
