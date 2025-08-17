using FluentValidation;

namespace YNAER.Application.Features.BankAccounts.DeleteBankAccount;

public class DeleteBankAccountValidator : AbstractValidator<DeleteBankAccountCommand>
{
    public DeleteBankAccountValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}