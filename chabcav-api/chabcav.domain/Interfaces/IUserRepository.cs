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

        Task<User> AuthenticateAsync(string email, string password);
    }

}
