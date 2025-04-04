using chabcav.domain.Aggregates.Models;
using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using chabcav.domain.Services;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    }

  
}
