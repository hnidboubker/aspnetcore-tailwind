using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TailwindIdentity.Core.Models;

namespace TailwindMvc.Web.Controllers;

[Authorize]
public class ManageController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<ManageController> _logger;

    public ManageController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<ManageController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            StatusMessage = "Erreur lors de la mise à jour du profil.";
            return RedirectToAction(nameof(Index));
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Profil mis à jour avec succès.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        _logger.LogInformation("User changed their password successfully.");
        StatusMessage = "Mot de passe mis à jour avec succès.";
        return RedirectToAction(nameof(ChangePassword));
    }
}

public class ProfileViewModel
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

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Le mot de passe actuel est requis")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe actuel")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nouveau mot de passe est requis")]
    [StringLength(100, ErrorMessage = "Le mot de passe doit contenir entre {2} et {1} caractères.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Nouveau mot de passe")]
    public string NewPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirmer le mot de passe")]
    [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
