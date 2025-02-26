using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbConnection _dbConnection;

        public RoleRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Role GetRoleByName(string roleName)
        {
            var role =  _dbConnection.QueryFirstOrDefault<Role>("SELECT * FROM aspnetroles WHERE Name = @Name", new { Name = roleName });

            return role;
        }
    }
}
