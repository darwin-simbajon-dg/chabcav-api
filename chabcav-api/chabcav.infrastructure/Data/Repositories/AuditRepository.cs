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
        private readonly Func<IDbConnection> _dbConnectionFactory;

        public AuditRepository(Func<IDbConnection> dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Activity>> GetAll()
        {
            try
            {
                using (var connection = _dbConnectionFactory())
                {
                    return await connection.GetAllAsync<Activity>();

                }
                   
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
                using (var connection = _dbConnectionFactory())
                {
                    _ = connection.Insert<Activity>(activity);
                }

                return Task.CompletedTask;
               
            }
            catch (Exception ex)
            {

                return Task.CompletedTask;
            }
        }
    }
}
