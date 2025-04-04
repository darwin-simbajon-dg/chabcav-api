using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IDbConnection _dbConnection;

        public AuditRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Activity>> GetAll()
        {
            try
            {
                return await _dbConnection.GetAllAsync<Activity>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task LogActivity(Activity activity)
        {
            try
            {
                _ = _dbConnection.Insert<Activity>(activity);

                return Task.CompletedTask;
               
            }
            catch (Exception ex)
            {

                return Task.CompletedTask;
            }
        }
    }
}
