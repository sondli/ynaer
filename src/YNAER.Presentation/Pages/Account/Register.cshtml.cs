using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YNAER.Domain.Entities;

namespace YNAER.Presentation.Pages.Account;

public class Register : PageModel
{
    private readonly ILogger<Register> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public Register(ILogger<Register> logger, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnPostAsync(string email, string password, string confirmPassword,
        string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            return Partial("_StatusMessagePartial", "Invalid register info");
        }

        if (password != confirmPassword)
        {
            return Partial("_StatusMessagePartial", "Passwords do not match");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            return Partial("_StatusMessagePartial", errors);
        }

        _logger.LogInformation("User created a new account with password.");

        await _signInManager.SignInAsync(user, isPersistent: false);

        Response.Headers["HX-Redirect"] = returnUrl;
        return new EmptyResult();
    }
}