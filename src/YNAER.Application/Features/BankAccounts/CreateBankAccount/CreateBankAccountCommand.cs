using YNAER.Application.Abstractions.Common;
using YNAER.Domain.Entities;

namespace YNAER.Application.Features.BankAccounts.CreateBankAccount;

public record CreateBankAccountCommand(ApplicationUser User, string Name) : ICommand<BankAccount>;