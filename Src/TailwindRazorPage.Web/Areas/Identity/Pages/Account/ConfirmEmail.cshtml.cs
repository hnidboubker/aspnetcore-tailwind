using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TailwindIdentity.Core.Models;

namespace TailwindIdentity.Core.Areas.Identity.Pages.Account;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string? userId, string? code)
    {
        if (userId == null || code == null)
        {
            return RedirectToPage("/Index", new { area = "" });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        StatusMessage = result.Succeeded ? "Email confirmed." : "Error confirming email.";

        return Page();
    }
}
