using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TailwindIdentity.Core.Models;

namespace TailwindIdentity.Core.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public string Username { get; set; } = string.Empty;

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Le prénom est requis")]
        [MaxLength(100)]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis")]
        [MaxLength(100)]
        [Display(Name = "Nom")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        Username = user.UserName!;

        Input = new InputModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        user.FirstName = Input.FirstName;
        user.LastName = Input.LastName;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            StatusMessage = "Erreur lors de la mise à jour du profil.";
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Profil mis à jour avec succès.";
        return RedirectToPage();
    }
}
