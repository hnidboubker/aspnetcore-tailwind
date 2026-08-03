using System.ComponentModel.DataAnnotations;
using Hasim.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TailwindRazorPage.Web.Pages.Account;

public class SignInPageModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public SignInPageModel(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [TempData]
    public string? ErrorMessage { get; set; }

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Display(Name = "Nom d'utilisateur ou e-mail")]
        [Required(ErrorMessage = "Le nom d'utilisateur ou l'e-mail est requis.")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }

    public IActionResult OnGet(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return LocalRedirect(returnUrl ?? Url.Page("/Index")!);
        }

        ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await FindUserAsync(Input.UserNameOrEmail);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Nom d'utilisateur/e-mail ou mot de passe invalide.");
            return Page();
        }

        var result = await _signInManager.PasswordSignInAsync(
            user, Input.Password, Input.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToPage("/Index");
        }

        if (result.RequiresTwoFactor)
        {
            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        }

        if (result.IsLockedOut)
        {
            return RedirectToPage("./Lockout");
        }

        ModelState.AddModelError(string.Empty, "Nom d'utilisateur/e-mail ou mot de passe invalide.");
        return Page();
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }

    private async Task<AppUser?> FindUserAsync(string userNameOrEmail)
    {
        if (userNameOrEmail.Contains('@'))
        {
            return await _userManager.FindByEmailAsync(userNameOrEmail);
        }

        return await _userManager.FindByNameAsync(userNameOrEmail);
    }
}
