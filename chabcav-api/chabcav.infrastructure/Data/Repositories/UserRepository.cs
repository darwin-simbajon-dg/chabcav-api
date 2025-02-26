using chabcav.domain.Aggregates.Models;
using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using chabcav.domain.Services;
using Dapper;
using MediatR;
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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IDbConnection _dbConnection;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRepository(UserManager<IdentityUser> userManager, 
            IPasswordHasher passwordHasher, 
            IDbConnection dbConnection,
            IUserRoleRepository userRoleRepository)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _dbConnection = dbConnection;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<RegistrationResult> AddAsync(User user)
        {
            try
            {
                var newUser = new IdentityUser
                {
                    UserName = user.Username,
                    NormalizedUserName = user.Username,
                    Email = user.Email,
                    NormalizedEmail = user.Email,
                    PasswordHash = user.PasswordHash
                };

                var result = await _userManager.CreateAsync(newUser, user.PasswordHash);

                if (result.Succeeded)
                {
                    _userRoleRepository.AddUserToRoleAsync(Guid.Parse(newUser.Id), user.Role);

                    

                    return new RegistrationResult()
                    {
                        IsSuccessful = true
                    };
                }
                return new RegistrationResult()
                {
                    IsSuccessful = false,
                    Message = result.Errors.FirstOrDefault().Description
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            //var user = await GetByEmailAsync(email);

            try
            {

                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid email or password");
                }

                //var identityUser = new IdentityUser
                //{
                //    UserName = user.UserName,
                //    Email = user.Email,
                //    PasswordHash = user.PasswordHash
                //};
                //var hashedPassword = _passwordHasher.HashPassword(password);

                var passwordValid = _passwordHasher.VerifyPassword(user.PasswordHash, password);

                if (!passwordValid)
                {
                    throw new UnauthorizedAccessException("Invalid email or password");
                }

                var userRole = await _userRoleRepository.GetRoleByUserId(Guid.Parse(user.Id));

                return new User(user.UserName, user.Email, user.PasswordHash, userRole.name);
            }
            catch (Exception ex)
            {

                throw ex;
            }

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
