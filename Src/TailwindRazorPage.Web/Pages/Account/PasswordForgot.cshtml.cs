using System.ComponentModel.DataAnnotations;
using System.Text;
using Hasim.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace TailwindRazorPage.Web.Pages.Account;

public class PasswordForgotModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;

    public PasswordForgotModel(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ConfirmationMessage { get; set; }

    public string? ResetLink { get; set; }

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

            ResetLink = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);

            ConfirmationMessage = "Un lien de réinitialisation a été généré.";
        }
        else
        {
            ConfirmationMessage = "Si un compte est associé à cette adresse, un lien de réinitialisation sera fourni.";
        }

        return Page();
    }
}
