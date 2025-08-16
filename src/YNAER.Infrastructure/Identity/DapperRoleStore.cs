using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using YNAER.Infrastructure.Persistence;

namespace YNAER.Infrastructure.Identity;

public class DapperRoleStore : IRoleStore<IdentityRole<Guid>>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperRoleStore(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    public void Dispose()
    {
    }

    public async Task<IdentityResult> CreateAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
                           insert into ynaer."AspNetRoles" 
                               ("Id", "Name", "NormalizedName", "ConcurrencyStamp")
                               values 
                               (@Id, @Name, @NormalizedName, @ConcurrencyStamp);
                           """;

        var rowsAffected = await connection.ExecuteAsync(sql, role);

        return rowsAffected == 0
            ? IdentityResult.Failed()
            : IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
                           update ynaer."AspNetRoles" set
                           "Name" = @Name,
                           "NormalizedName" = @NormalizedName,
                           "ConcurrencyStamp" = @ConcurrencyStamp
                           where "Id" = @Id;
                           """;

        var rowsAffected = await connection.ExecuteAsync(sql, role);

        return rowsAffected == 0
            ? IdentityResult.Failed()
            : IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """delete from ynaer."AspNetRoles" where "Id" = @Id;""";

        await connection.ExecuteAsync(sql, new { role.Id });
        return IdentityResult.Success;
    }

    public Task<string> GetRoleIdAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(role.Name);
    }

    public Task SetRoleNameAsync(IdentityRole<Guid> role, string? roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.Name = roleName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedRoleNameAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(role.NormalizedName);
    }

    public Task SetNormalizedRoleNameAsync(IdentityRole<Guid> role, string? normalizedName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityRole<Guid>?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """select * from ynaer."AspNetRoles" where "Id" = @Id;""";

        return await connection.QuerySingleOrDefaultAsync<IdentityRole<Guid>?>(sql, new { Id = roleId });
    }

    public async Task<IdentityRole<Guid>?> FindByNameAsync(string normalizedRoleName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """select * from ynaer."AspNetRoles" where "NormalizedName" = @NormalizedName;""";

        return await connection.QuerySingleOrDefaultAsync<IdentityRole<Guid>?>(
            sql, new { NormalizedName = normalizedRoleName }
        );
    }
}