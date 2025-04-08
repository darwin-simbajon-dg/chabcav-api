using chabcav.domain.Aggregates.Models;
using chabcav.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<RegistrationResult> AddAsync(User user);
        Task<bool> UpdatePassword(Guid userId, string currentPassword, string password);
        Task<bool> ResetPassword(string email, string currentPassword, string newPassword);
        Task<User> AuthenticateAsync(string email, string password);
        Task<bool> SaveOTP(string emailAddress, string otp);

        Task<bool> VerifyOTP(string emailAddress, string otp);
    }

}
