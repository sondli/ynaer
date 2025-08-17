using YNAER.Domain.Entities;

namespace YNAER.Domain.Abstractions;

public interface IBankAccountsRepository
{
    Task<IEnumerable<BankAccount>> ListByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<BankAccount?> GetAsync(Guid accountId, CancellationToken ct = default);
    Task<BankAccount> AddAsync(BankAccount account, CancellationToken ct = default);
    Task DeleteAsync(BankAccount account, CancellationToken ct = default);
}