using chabcav.domain.Aggregates.Models;
using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using chabcav.domain.Services;
using chabcav.infrastructure.Data.Entity;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = chabcav.domain.Entities.User;

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

                return new User(Guid.Parse(user.Id), user.UserName, user.Email, user.PasswordHash, userRole.name);
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

        public async Task<bool> ResetPassword(string email, string currentPassword, string newPassword)
        {
            try
            {
                //const string query = "SELECT * FROM aspnetUsers WHERE email = @Email";
                //var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
                // Hash the new password
                var hashedPassword = _passwordHasher.HashPassword(newPassword);

                // Define the SQL query to update the password hash
                const string sql = "UPDATE AspNetUsers SET PasswordHash = @PasswordHash WHERE Email = @Email";

                // Execute the query
                var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { PasswordHash = hashedPassword, Email = email });

                // Return true if the update was successful
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }

        public async Task<bool> SaveOTP(string emailAddress, string otpkey)
        {
            try
            {
                const string sql = "UPDATE AspNetUsers SET otp = @otp WHERE email = @email";

                // Execute the query
                var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { otp = otpkey, email = emailAddress });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> UpdatePassword(Guid userId, string newPassword, string password)
        {
            var foundUser = await _userManager.FindByIdAsync(userId.ToString());
            try
            {
                // Hash the new password
                var hashedPassword = _passwordHasher.HashPassword(newPassword);

                // Define the SQL query to update the password hash
                const string sql = "UPDATE AspNetUsers SET PasswordHash = @PasswordHash WHERE Id = @Id";

                // Execute the query
                var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { PasswordHash = hashedPassword, Id = userId });

                // Return true if the update was successful
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }

        public async Task<bool> VerifyOTP(string emailAddress, string otp)
        {
            try
            {
                const string sql = "SELECT otp FROM AspNetUsers WHERE email = @Email";
                var storedOtp = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new { Email = emailAddress });

                return storedOtp == otp;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }
    }

  
}
