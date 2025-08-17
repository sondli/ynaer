using FluentValidation;

namespace YNAER.Application.Features.BankAccounts.GetBankAccount;

public class GetBankAccountValidator : AbstractValidator<GetBankAccountQuery>
{
    public GetBankAccountValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
    }
}