using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YNAER.Application.Features.BankAccounts.CreateBankAccount;
using YNAER.Application.Features.BankAccounts.ListBankAccounts;
using YNAER.Domain.Entities;

namespace YNAER.Presentation.Pages;

[Authorize]
public class Overview : PageModel
{
    private readonly ListBankAccountsHandler _listBankAccountsHandler;
    private readonly CreateBankAccountHandler _createBankAccountHandler;
    private readonly UserManager<ApplicationUser> _userManager;

    public Overview(ListBankAccountsHandler listBankAccountsHandler, UserManager<ApplicationUser> userManager,
        CreateBankAccountHandler createBankAccountHandler)
    {
        _listBankAccountsHandler = listBankAccountsHandler;
        _userManager = userManager;
        _createBankAccountHandler = createBankAccountHandler;
    }

    public List<BankAccount> Accounts { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var query = new ListBankAccountsQuery(user);
        var result = await _listBankAccountsHandler.HandleAsync(query);
        Accounts = result switch
        {
            { IsSuccess: true, Value: var accounts } => accounts.ToList(),
            _ => Accounts
        };

        return Page();
    }

    public IActionResult OnGetNewAccountForm()
    {
        return Partial("_NewAccountForm");
    }

    public async Task<IActionResult> OnPostNewAccountAsync(string accountName)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var command = new CreateBankAccountCommand(user, accountName);
        var result = await _createBankAccountHandler.HandleAsync(command);

        switch (result)
        {
            case { IsSuccess: true, Value: var account }:
            {
                Accounts.Add(account);
                break;
            }
            case { IsFailure: true, Error: var error }:
            {
                return Partial("_StatusMessagePartial", error.Message);
            }
        }

        return Page();
    }
}