using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YNAER.Domain.Entities;

namespace YNAER.Presentation.Pages.Account;

public class Login : PageModel
{
    private readonly ILogger<Login> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public Login(ILogger<Login> logger, SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _signInManager = signInManager;
    }

    public string? ReturnUrl { get; set; }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string userName, string password, bool rememberMe = false,
        string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            return Partial("_LoginStatusMessage", "Invalid login info");
        }

        var result =
            await _signInManager.PasswordSignInAsync(userName, password, rememberMe, lockoutOnFailure: false);

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User is locked out");

            return Partial("_LoginStatusMessage", "Your account is locked");
        }

        if (!result.Succeeded)
        {
            return Partial("_LoginStatusMessage", "Login failed");
        }

        _logger.LogInformation("User logged in");
        Response.Headers["HX-Redirect"] = returnUrl;
        return new EmptyResult();
    }
}