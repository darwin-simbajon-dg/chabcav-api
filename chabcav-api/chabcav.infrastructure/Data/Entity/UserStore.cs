using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;

public class UserStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>
{
    private readonly IDbConnection _dbConnection;

    public UserStore(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        const string sql = "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash) VALUES (CAST(@Id AS UUID), @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @PasswordHash)";
        await _dbConnection.ExecuteAsync(sql, user);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM AspNetUsers WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, new { user.Id });
        return IdentityResult.Success;
    }

    public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM AspNetUsers WHERE Id = @Id";
        return await _dbConnection.QuerySingleOrDefaultAsync<IdentityUser>(sql, new { Id = userId });
    }

    public async Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM AspNetUsers WHERE NormalizedUserName = @NormalizedUserName";
        return await _dbConnection.QuerySingleOrDefaultAsync<IdentityUser>(sql, new { NormalizedUserName = normalizedUserName });
    }

    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id);

    public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);

    public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(user.NormalizedUserName);

    public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash);

    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

    public void Dispose() { /* Dispose resources if necessary */ }

    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
