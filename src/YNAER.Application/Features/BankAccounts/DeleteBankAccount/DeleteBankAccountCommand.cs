using YNAER.Application.Abstractions.Common;

namespace YNAER.Application.Features.BankAccounts.DeleteBankAccount;

public record DeleteBankAccountCommand(Guid Id) : ICommand;