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

        public async Task<Profile> GetProfileByEmailAsync(string email)
        {
            try
            {

                var profile = _dbConnection.QueryFirstOrDefault<Profile>("SELECT * FROM profiles WHERE email = @Email", new { Email = email });
                return profile;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public Task<bool> UpdateProfileAsync(Profile profile)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> UpdateProfileImage(Guid userId, string imageId)
        {
            try
            {
                var affectedRows = await _dbConnection
                    .ExecuteAsync("UPDATE profiles SET imageid = @ImageId WHERE id = @UserId", new { ImageId = imageId, UserId = userId });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<Guid> UpdateProfileAsync(Profile profile)
        {
            Guid profileId = Guid.Empty;
            int affectedRows = 0;

            try
            {
                var foundProfile = await _dbConnection.GetAsync<Profile>(profile.id);

                if (foundProfile is null)
                {
                    affectedRows = await _dbConnection.ExecuteAsync(@"
                        INSERT INTO profiles (id, information, fullname, email, phonenumber, location, birthdate, imageid)
                        VALUES (@Id, @Information, @Fullname, @Email, @Phonenumber, @Location, @Birthdate, @Imageid)", profile);

                    return profile.id;
                }

                _ = await _dbConnection.UpdateAsync<Profile>(profile);

                return profile.id;


            }
            catch (Exception ex)
            {

                return Guid.Empty;
            }
        }

        public Task<IEnumerable<Profile>> GetAllProfilesAsync()
        {
            try
            {
                return _dbConnection.QueryAsync<Profile>("SELECT * FROM profiles");
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
