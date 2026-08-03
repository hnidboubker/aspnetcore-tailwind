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

public class SignUpPageModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public SignUpPageModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _configuration = configuration;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Display(Name = "Prénom")]
        [Required(ErrorMessage = "Le prénom est requis.")]
        [StringLength(100, ErrorMessage = "Le prénom ne doit pas dépasser 100 caractères.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(100, ErrorMessage = "Le nom ne doit pas dépasser 100 caractères.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Adresse e-mail")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail est invalide.")]
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, ErrorMessage = "Le mot de passe doit comporter au moins {2} caractères.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas.")]
        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new AppUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            FirstName = Input.FirstName,
            LastName = Input.LastName
        };

        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            if (_configuration.GetValue<bool>("Identity:RequireConfirmedEmail"))
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
                    <h2>Bienvenue sur TailwindRazorPage</h2>
                    <p>Bonjour {HtmlEncoder.Default.Encode(Input.FirstName)},</p>
                    <p>Merci de confirmer votre adresse e-mail en cliquant sur le lien ci-dessous :</p>
                    <p><a href="{HtmlEncoder.Default.Encode(confirmLink ?? string.Empty)}">Confirmer mon adresse e-mail</a></p>
                    <p>Si vous n'êtes pas à l'origine de cette inscription, ignorez cet e-mail.</p>
                    """;

                await _emailService.SendAsync(Input.Email, subject, html);

                return RedirectToPage("/Account/SendConfirmation", new { });
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
