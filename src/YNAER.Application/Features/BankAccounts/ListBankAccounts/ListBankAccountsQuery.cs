using YNAER.Application.Abstractions.Common;
using YNAER.Domain.Entities;

namespace YNAER.Application.Features.BankAccounts.ListBankAccounts;

public record ListBankAccountsQuery(ApplicationUser User) : IQuery<IEnumerable<BankAccount>>;