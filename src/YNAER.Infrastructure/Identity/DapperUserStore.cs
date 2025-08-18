using Dapper;
using Microsoft.AspNetCore.Identity;
using YNAER.Domain.Entities;
using YNAER.Infrastructure.Persistence;

namespace YNAER.Infrastructure.Identity;

public class DapperUserStore : IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperUserStore(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Dispose()
    {
    }

    public Task<string> GetUserIdAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(applicationUser.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(applicationUser.UserName);
    }

    public Task SetUserNameAsync(ApplicationUser applicationUser, string? userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        applicationUser.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser applicationUser,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(applicationUser.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser applicationUser, string? normalizedName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        applicationUser.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (applicationUser.Id == Guid.Empty)
        {
            applicationUser.Id = Guid.CreateVersion7();
        }
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
                           insert into ynaer."AspNetUsers" 
                           (
                           "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", 
                           "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", 
                           "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount"
                           ) values (
                           @Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, 
                           @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, 
                           @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount
                           );
                           """;

        var rowsAffected = await connection.ExecuteAsync(sql, applicationUser);

        return rowsAffected == 0
            ? IdentityResult.Failed()
            : IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
                           update ynaer."AspNetUsers" set
                             "UserName" = @UserName,
                             "NormalizedUserName" = @NormalizedUserName,
                             "Email" = @Email,
                             "NormalizedEmail" = @NormalizedEmail,
                             "EmailConfirmed" = @EmailConfirmed,
                             "PasswordHash" = @PasswordHash,
                             "SecurityStamp" = @SecurityStamp,
                             "ConcurrencyStamp" = @ConcurrencyStamp,
                             "PhoneNumber" = @PhoneNumber,
                             "PhoneNumberConfirmed" = @PhoneNumberConfirmed,
                             "TwoFactorEnabled" = @TwoFactorEnabled,
                             "LockoutEnd" = @LockoutEnd,
                             "LockoutEnabled" = @LockoutEnabled,
                             "AccessFailedCount" = @AccessFailedCount
                           where "Id" = @Id;
                           """;

        var rowsAffected = await connection.ExecuteAsync(sql, applicationUser);

        return rowsAffected == 0
            ? IdentityResult.Failed()
            : IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """delete from ynaer."AspNetUsers" where "Id" = @Id;""";
        await connection.ExecuteAsync(sql, new { applicationUser.Id });

        return IdentityResult.Success;
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """select * from ynaer."AspNetUsers" where "Id" = @Id;""";
        return await connection.QuerySingleOrDefaultAsync<ApplicationUser?>(sql, new { Id = new Guid(userId) });
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """select * from ynaer."AspNetUsers" where "NormalizedUserName" = @NormalizedUserName;""";
        return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(
            sql, new { NormalizedUserName = normalizedUserName }
        );
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.PasswordHash is not null);
    }

    public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """select * from ynaer."AspNetUsers" where "NormalizedEmail" = @NormalizedEmail;""";
        return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(
            sql, new { NormalizedEmail = normalizedEmail }
        );
    }

    public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }
}