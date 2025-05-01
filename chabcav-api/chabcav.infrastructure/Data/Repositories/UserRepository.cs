using chabcav.domain.Aggregates.Models;
using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using chabcav.domain.Services;
using chabcav.infrastructure.Data.Entity;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IProfileRepository _profileRepository;
        private readonly IAuditRepository _auditRepository;

        public UserRepository(UserManager<IdentityUser> userManager,
            IPasswordHasher passwordHasher,
            IDbConnection dbConnection,
            IUserRoleRepository userRoleRepository,
            IProfileRepository profileRepository,
            IAuditRepository auditRepository)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _dbConnection = dbConnection;
            _userRoleRepository = userRoleRepository;
            _profileRepository = profileRepository;
            _auditRepository = auditRepository;
        }

        /*public async Task<RegistrationResult> AddAsync(User user)
        {
            try
            {
                var newUser = new IdentityUser
                {
                    UserName = user.Username,
                    NormalizedUserName = user.Username.ToUpper(),
                    Email = user.Email,
                    NormalizedEmail = user.Email.ToUpper(),
                    PasswordHash = user.PasswordHash
                };

                var result = await _userManager.CreateAsync(newUser, user.PasswordHash);

                if (result.Succeeded)
                {
                    // Assign role
                    await _userRoleRepository.AddUserToRoleAsync(Guid.Parse(newUser.Id), user.Role);

                    // Add profile
                    await AddProfile(user);

                    // Insert into usersprogress table
                    const string insertProgressSql = @"
                INSERT INTO usersprogress (usersid, usersname) 
                VALUES (@UsersId, @UsersName);";

                    await _dbConnection.ExecuteAsync(insertProgressSql, new
                    {
                        UsersId = newUser.Id,
                        UsersName = newUser.UserName
                    });

                    return new RegistrationResult()
                    {
                        IsSuccessful = true
                    };
                }

                return new RegistrationResult()
                {
                    IsSuccessful = false,
                    Message = result.Errors.FirstOrDefault()?.Description
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }*/


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

                     await AddProfile(user);

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

        public async Task AddProfile(User user)
        {
            try
            {
                var profile = new Profile()
                {
                    id = user.Id,
                    birthdate = DateTime.Now,
                    email = user.Email,
                    fullname = user.Username,
                    location = "Philippines"
                };

                var result = await _profileRepository.AddProfileAsync(profile);
            }
            catch (Exception ex)
            {

                
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

                var activity = new Activity()
                {
                    action = "LOGIN",
                    userid = Guid.Parse(user.Id),
                    actiondate = DateTime.Now
                };

                await _auditRepository.LogActivity(activity);

                return new User(Guid.Parse(user.Id), user.UserName, user.Email, user.PasswordHash, userRole.name);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await _dbConnection.QueryAsync<User>("SELECT * FROM aspnetUsers");
            
            return users.ToList();
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

        //For Admin Users Management
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                const string sql = @"
            UPDATE AspNetUsers 
            SET UserName = @Username,
                NormalizedUserName = @Username,
                Email = @Email,
                NormalizedEmail = @Email
            WHERE Id = @Id";

                var rowsAffected = await _dbConnection.ExecuteAsync(sql, new
                {
                    user.Username,
                    user.Email,
                    user.Id
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            using var transaction = _dbConnection.BeginTransaction();

            try
            {
                // Delete from roles first (assuming FK constraint)
                await _dbConnection.ExecuteAsync("DELETE FROM AspNetUserRoles WHERE UserId = @Id", new { Id = userId }, transaction);

                // Delete from profiles
                await _dbConnection.ExecuteAsync("DELETE FROM Profiles WHERE Id = @Id", new { Id = userId }, transaction);

                // Finally, delete from AspNetUsers
                await _dbConnection.ExecuteAsync("DELETE FROM AspNetUsers WHERE Id = @Id", new { Id = userId }, transaction);

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }





    }


}
