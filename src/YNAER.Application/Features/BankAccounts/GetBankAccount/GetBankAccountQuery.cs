using YNAER.Application.Abstractions.Common;
using YNAER.Domain.Entities;

namespace YNAER.Application.Features.BankAccounts.GetBankAccount;

public record GetBankAccountQuery(Guid AccountId) : IQuery<BankAccount>;