﻿using chabcav.domain.Entities;
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
    public class ProfileRepository : IProfileRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProfileRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
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

        public Task<bool> UpdateProfileAsync(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
