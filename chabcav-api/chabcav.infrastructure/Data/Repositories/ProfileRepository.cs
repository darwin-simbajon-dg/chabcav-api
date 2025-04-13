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
using chabcav.domain.Interfaces;
using chabcav.domain.Entities;
using System.Data;
using Dapper.Contrib.Extensions;

namespace chabcav.infrastructure.Data.Repositories
{



    public class ProfileRepository : IProfileRepository
    {
        private readonly Func<IDbConnection> _dbConnectionFactory;

        public ProfileRepository(Func<IDbConnection> dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Guid> AddProfileAsync(Profile profile)
        {
            try
            {
                using (var connection = _dbConnectionFactory())
                {
                    connection.Open();
                    var sql = @"
                        INSERT INTO profiles (information, fullname, email, phonenumber, location, birthdate)
                        VALUES (@Information, @Fullname, @Email, @Phonenumber, @Location, @Birthdate)
                        RETURNING id;";

                    profile.id = await connection.ExecuteScalarAsync<Guid>(sql, profile);
                    return profile.id;
                }
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        public async Task<Profile> GetProfileAsync(Guid userId)
        {
            try
            {
                using (var connection = _dbConnectionFactory())
                {
                    connection.Open();
                    var profile = await connection.GetAsync<Profile>(userId);
                    return profile ?? throw new InvalidOperationException("Profile not found");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Profile> GetProfileByEmailAsync(string email)
        {
            try
            {
                using (var connection = _dbConnectionFactory())
                {
                    connection.Open();
                    var profile = await connection.QueryFirstOrDefaultAsync<Profile>("SELECT * FROM profiles WHERE email = @Email", new { Email = email });
                    return profile ?? throw new InvalidOperationException("Profile not found");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateProfileImage(Guid userId, string imageId)
        {
            try
            {
                using (var connection = _dbConnectionFactory())
                {
                    connection.Open();
                    var affectedRows = await connection.ExecuteAsync("UPDATE profiles SET imageid = @ImageId WHERE id = @UserId", new { ImageId = imageId, UserId = userId });
                    return affectedRows > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Guid> UpdateProfileAsync(Profile profile)
        {
            try
            {
                using (var connection = _dbConnectionFactory())
                {
                    connection.Open();
                    var foundProfile = await connection.GetAsync<Profile>(profile.id);
                    if (foundProfile is null)
                    {
                        await connection.ExecuteAsync(@"
                                INSERT INTO profiles (id, information, fullname, email, phonenumber, location, birthdate, imageid)
                                VALUES (@Id, @Information, @Fullname, @Email, @Phonenumber, @Location, @Birthdate, @Imageid)", profile);
                    }
                    else
                    {
                        await connection.UpdateAsync(profile);
                    }
                    return profile.id;
                }
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        public async Task<IEnumerable<Profile>> GetAllProfilesAsync()
        {
            try
            {
                using (var connection = _dbConnectionFactory())
                {
                    connection.Open();
                    return await connection.GetAllAsync<Profile>();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }



}
