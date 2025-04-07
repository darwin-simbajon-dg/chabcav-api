using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;

public class UserStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>, IUserEmailStore<IdentityUser>
{
    private readonly IDbConnection _dbConnection;

    public UserStore(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        const string sql = "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash) VALUES (CAST(@Id AS UUID), @UserName, UPPER(@NormalizedUserName), @Email, @NormalizedEmail, @PasswordHash)";
        user.NormalizedEmail = user.NormalizedEmail.ToUpper();
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
        if (string.IsNullOrEmpty(passwordHash))
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
        
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash);

    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

    public void Dispose() { /* Dispose resources if necessary */ }

    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM AspNetUsers WHERE normalizedemail = @NormalizedEmail";
        var result = await _dbConnection.QuerySingleOrDefaultAsync<UserDto>(sql, new { NormalizedEmail = normalizedEmail });

        return new IdentityUser
        {
            Id = result.Id.ToString(), // Convert UUID to string if necessary
            UserName = result.UserName,
            NormalizedUserName = result.NormalizedUserName,
            Email = result.Email,
            NormalizedEmail = result.NormalizedEmail,
            PasswordHash = result.PasswordHash
        };
    }

    public Task SetEmailAsync(IdentityUser user, string? email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        const string sql = "SELECT email FROM AspNetUsers WHERE email = @NormalizedEmail";
        return await _dbConnection.QuerySingleOrDefaultAsync<string>(sql, new { NormalizedEmail = user.NormalizedEmail });
    }

    public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SetNormalizedEmailAsync(IdentityUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        // Ensure the cancellation token is respected
        cancellationToken.ThrowIfCancellationRequested();

        // Update the normalized email in the database
        //const string sql = @"UPDATE AspNetUsers 
        //                 SET NormalizedEmail = @NormalizedEmail
        //                 WHERE Id = @Id";

        //await _dbConnection.ExecuteAsync(sql, new
        //{
        //    NormalizedEmail = user.Email.ToUpper(),
        //    Id = user.Id
        //});

        //// Update the in-memory user object as well
        //user.NormalizedEmail = normalizedEmail;
    }
}

public class UserDto
{
    public Guid Id { get; set; } // Matches database type
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public string PasswordHash { get; set; }
}
