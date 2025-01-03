using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IDbConnection _dbConnection;

        public UserRepository(UserManager<IdentityUser> userManager, IDbConnection dbConnection)
        {
            _userManager = userManager;
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(User user)
        {
            var newUser = new IdentityUser
            {
                UserName = user.Username,
                NormalizedUserName = user.Username,
                Email = user.Email,
                NormalizedEmail = user.Email
            };

            await _userManager.CreateAsync(newUser, user.PasswordHash);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            //var user = await _userManager.FindByEmailAsync(email);

            //return new User(user.UserName, user.Email, user.PasswordHash);
            try
            {
                const string query = "SELECT * FROM aspnetUsers WHERE email = @Email";
                var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
                return user;
            }
            catch (Exception ex)
            {

                throw ex;
            }
          ;
        }
    }
}
