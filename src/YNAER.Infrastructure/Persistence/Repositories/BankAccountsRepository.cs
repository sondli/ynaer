using Dapper;
using YNAER.Domain.Abstractions;
using YNAER.Domain.Entities;

namespace YNAER.Infrastructure.Persistence.Repositories;

public class BankAccountsRepository : IBankAccountsRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public BankAccountsRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<BankAccount>> ListByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(ct);

        const string sql = """select * from ynaer."BankAccounts" where "UserId" = @UserId;""";

        return await connection.QueryAsync<BankAccount>(sql, new { UserId = userId });
    }

    public async Task<BankAccount?> GetAsync(Guid accountId, CancellationToken ct = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(ct);

        const string sql = """select * from ynaer."BankAccounts" where "Id" = @Id;""";

        return await connection.QuerySingleOrDefaultAsync<BankAccount>(sql, new { Id = accountId });
    }

    public async Task<BankAccount> AddAsync(BankAccount account, CancellationToken ct = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(ct);

        const string sql = """
                            insert into ynaer."BankAccounts" ("Id", "UserId", "Name", "CreatedOn") values (
                                @Id, @UserId, @Name, @CreatedOn
                            );
                           """;

        await connection.ExecuteAsync(sql, account);

        return account;
    }

    public async Task DeleteAsync(BankAccount account, CancellationToken ct = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(ct);

        const string sql = """delete from ynaer."BankAccounts" where "Id" = @Id""";

        await connection.ExecuteAsync(sql, new { account.Id });
    }
}