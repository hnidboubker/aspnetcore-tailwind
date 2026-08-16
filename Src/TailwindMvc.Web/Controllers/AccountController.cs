using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using TailwindIdentity.Core.Models;
using TailwindIdentity.EntityFrameworkCore.Services;
using TailwindMvc.Web.Models;

namespace TailwindMvc.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> SignInManager;
    private readonly UserManager<ApplicationUser> UserManager;
    private readonly IEmailService EmailService;
    private readonly ILogger<AccountController> Logger;

    public AccountController(SignInManager<ApplicationUser> signInManager,
                             UserManager<ApplicationUser> userManager,
                             ILogger<AccountController> logger,
                             IEmailService emailService)

    {
                            SignInManager = signInManager;
                            UserManager = userManager;
                            Logger = logger;
                            EmailService = emailService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var result = await SignInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                Logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl ?? "/");
            }
            if (result.IsLockedOut)
            {
                Logger.LogWarning("User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email ou mot de passe invalide.");
                return View(model);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult SignUp(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(RegisterViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                Logger.LogInformation("User created a new account with password.");
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var code = await UserManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { area = "", code = code }, protocol: Request.Scheme);

                Logger.LogInformation("Password reset token generated for user {Email}", model.Email);
            }

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    //[HttpGet]
    //public IActionResult ConfirmEmail(string? userId, string? code)
    //{
    //    if (userId == null || code == null)
    //    {
    //        return RedirectToAction("Index", "Home");
    //    }
    //    return View();
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await SignInManager.SignOutAsync();
        Logger.LogInformation("User logged out.");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SendConfirmation()
    {
        return View(new SendConfirmationViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendConfirmation(SendConfirmationViewModel model)       
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await UserManager.FindByEmailAsync(model.Email);

        // Toujours afficher un message générique
        // pour éviter l'énumération des utilisateurs.
        if (user is not null &&
            !await UserManager.IsEmailConfirmedAsync(user))
        {
            var code = await UserManager
                .GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(code));

            var confirmLink = Url.Action(
                "ConfirmEmail",
                "Account",
                new
                {
                    userId = user.Id,
                    code
                },
                protocol: Request.Scheme);

            var subject = "Confirmez votre adresse e-mail";

            var html = $"""
                <h2>Confirmation d'adresse e-mail</h2>
                <p>Bonjour,</p>
                <p>
                    Merci de confirmer votre adresse e-mail
                    en cliquant sur le lien ci-dessous :
                </p>
                <p>
                    <a href="{HtmlEncoder.Default.Encode(confirmLink ?? string.Empty)}">
                        Confirmer mon adresse e-mail
                    </a>
                </p>
                <p>
                    Si vous n'êtes pas à l'origine de cette demande,
                    ignorez cet e-mail.
                </p>
                """;

            await EmailService.SendAsync(model.Email, subject, html);
            
              
               
        }

        // Message volontairement identique quel que soit le cas.
        model.ConfirmationMessage =
            "Si un compte est associé à cette adresse, " +
            "un e-mail de confirmation sera envoyé.";

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(int? userId, string code)
    {
        if (string.IsNullOrEmpty(userId.ToString()) ||
            string.IsNullOrEmpty(code))
        {
            return BadRequest();
        }

        ApplicationUser user = await UserManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return NotFound();
        }

        //if (string.IsNullOrEmpty(userId.))
        //{
        //    return NotFound();
        //}

        //var user = await UserManager.FindByIdAsync(userId);


        //if (user is null)
        //{
        //    return NotFound();
        //}
        var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
          

        var result = await UserManager.ConfirmEmailAsync(user, decodedCode);
            
         

        if (result.Succeeded)
        {
            return View("ConfirmEmailSuccess");
        }

        return View("ConfirmEmailError");
    }
}


