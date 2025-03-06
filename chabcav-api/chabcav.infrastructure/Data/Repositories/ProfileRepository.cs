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
    public class ProfileRepository : IProfileRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProfileRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid> AddProfileAsync(Profile profile)
        {
            try
            {
                var sql = @"
            INSERT INTO profiles (information, fullname, email, phonenumber, location, birthdate)
            VALUES (@Information, @Fullname, @Email, @Phonenumber, @Location, @Birthdate)
            RETURNING id;";

                profile.id = await _dbConnection.ExecuteScalarAsync<Guid>(sql, profile);

                return profile.id;

            }
            catch (Exception ex)
            {

                return Guid.Empty;
            }
        }

        public Task<Profile> GetProfileAsync(Guid userId)
        {
            try
            {
                var profile = _dbConnection.Get<Profile>(userId);

                return Task.FromResult(profile);
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<Guid> UpdateProfileAsync(Profile profile)
        {
            try
            {
                var addedProfile = await _dbConnection.UpdateAsync<Profile>(profile);

                return profile.id;

            }
            catch (Exception ex)
            {

                return Guid.Empty;
            }
        }
    }
}
