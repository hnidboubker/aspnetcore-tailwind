using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TailwindIdentity.Core.Models;

namespace TailwindIdentity.Core.Areas.Identity.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        public string Email { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { area = "Identity", code = code }, protocol: Request.Scheme);

                await _userManager.ResetPasswordAsync(user, code, "Temp@123");
            }

            return RedirectToAction("./ForgotPasswordConfirmation");
        }

        return Page();
    }
}
