using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using chabcav.infrastructure.Data.Entity;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Repositories
{
    public class CMSRepository : ICMSRepository
    {
        private readonly IDbConnection _dbConnection;

        public CMSRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection; 
        }

        public async Task<int> CreateConfiguration(Configuration configuration)
        {
            try
            {
                var mainConfiguration = new MainConfiguration()
                {
                    //id = Guid.NewGuid(),
                    bannerimage = configuration.BannerImage,
                    backgroundimage = configuration.BackgroundImage,
                    content = configuration.Content
                };

                var id = await _dbConnection.InsertAsync<MainConfiguration>(mainConfiguration);

                return id;
            }
            catch (Exception ex)
            {

                throw ex;
            }        
        }

        public Configuration GetConfiguration()
        {
            var configuration =  _dbConnection.GetAll<MainConfiguration>();

            if (configuration != null && configuration.Count() > 0)
            {
                var mainConfiguration = configuration.FirstOrDefault();

                return new Configuration()
                {
                    BannerImage = mainConfiguration.bannerimage,
                    BackgroundImage = mainConfiguration.backgroundimage,
                    Content = mainConfiguration.content
                };
            }

            return null;
        }
    }
}
