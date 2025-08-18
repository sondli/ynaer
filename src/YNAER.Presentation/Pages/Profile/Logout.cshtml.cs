using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YNAER.Domain.Entities;

namespace YNAER.Presentation.Pages.Profile;

public class Logout : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<Logout> _logger;

    public Logout(SignInManager<ApplicationUser> signInManager, ILogger<Logout> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");

        return LocalRedirect(returnUrl ?? Url.Content("~/"));
    }
}