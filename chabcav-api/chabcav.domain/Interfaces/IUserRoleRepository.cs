using chabcav.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Interfaces
{
    public interface IUserRoleRepository
    {
        public Task<UserRole> AddUserToRoleAsync(Guid userId, string roleName);
        public Task<Role> GetRoleByUserId(Guid userId);
    }
}
