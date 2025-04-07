using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IRoleRepository _roleRepository;

        public UserRoleRepository(IDbConnection dbConnection, IRoleRepository roleRepository)
        {
            _dbConnection = dbConnection;
            _roleRepository = roleRepository;
        }

        public async Task<UserRole> AddUserToRoleAsync(Guid userId, string roleName)
        {
            try
            {
                var role = _roleRepository.GetRoleByName(roleName);

                if (role != null)
                {
                    var userRole = new UserRole { userid = userId, roleid = role.id };
                    await _dbConnection.InsertAsync(userRole);
                    return userRole;
                }

                return null;
            }
            catch (Exception ex)
            {
                // Log the exception
                return null;
            }
        }

        public async Task<Role> GetRoleByUserId(Guid userId)
        {

                var userRole = await _dbConnection.QueryFirstOrDefaultAsync<UserRole>("SELECT * FROM aspnetuserroles WHERE UserId = @UserId", new { UserId = userId });

                var role = await _dbConnection.QueryFirstOrDefaultAsync<Role>("SELECT * FROM aspnetroles WHERE id = @RoleId", new { RoleId = userRole.roleid });

                return role;
    
        }
    }
}
