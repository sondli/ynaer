using FluentValidation;

namespace YNAER.Application.Features.BankAccounts.CreateBankAccount;

public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountCommand>
{
    public CreateBankAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.User).NotNull();
    }
}