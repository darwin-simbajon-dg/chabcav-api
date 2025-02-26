using chabcav.infrastructure.Data.Abstractions;
using chabcav.infrastructure.Data.Connections;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly IDbConnection _connection;

        public GenericRepository(PostgresConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _connection.GetAllAsync<T>();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _connection.GetAsync<T>(id);
        }

        public async Task AddAsync(T entity)
        {
            await _connection.InsertAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _connection.UpdateAsync(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await _connection.DeleteAsync(entity);
            }
        }
    }
}
