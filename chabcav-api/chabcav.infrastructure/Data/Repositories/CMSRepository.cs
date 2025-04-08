using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using chabcav.infrastructure.Data.Entity;
using Dapper;
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

        public async Task<bool> UpdateCMSImages(CMS cms)
        {
            try
            {
                var existingCms = await _dbConnection.QueryFirstOrDefaultAsync<CMS>("SELECT * FROM cms WHERE banner = @Banner", new { cms.banner });

                if (existingCms == null)
                {
                    var insertQuery = @"INSERT INTO cms (banner, midcontentimage, headline, content, card1, card2, card3, card4, card5, card6, card7, card8) 
                                        VALUES (@Banner, @MidContentImage, @Headline, @Content, @Card1, @Card2, @Card3, @Card4, @Card5, @Card6, @Card7, @Card8)";

                    var insertResult = await _dbConnection.ExecuteAsync(insertQuery, new
                    {
                        cms.banner,
                        cms.midcontentimage,
                        cms.headline,
                        cms.content,
                        cms.card1,
                        cms.card2,
                        cms.card3,
                        cms.card4,
                        cms.card5,
                        cms.card6,
                        cms.card7,
                        cms.card8
                    });

                    return insertResult > 0;
                }
                else
                {
                    existingCms.banner = cms.banner;
                    existingCms.midcontentimage = cms.midcontentimage;
                    existingCms.headline = cms.headline;
                    existingCms.content = cms.content;
                    existingCms.card1 = cms.card1;
                    existingCms.card2 = cms.card2;
                    existingCms.card3 = cms.card3;
                    existingCms.card4 = cms.card4;
                    existingCms.card5 = cms.card5;
                    existingCms.card6 = cms.card6;
                    existingCms.card7 = cms.card7;
                    existingCms.card8 = cms.card8;
                    //var updateQuery = @"UPDATE cms 
                    //                    SET midcontentimage = @MidContentImage, 
                    //                        headline = @Headline, 
                    //                        content = @Content, 
                    //                        card1 = @Card1, 
                    //                        card2 = @Card2, 
                    //                        card3 = @Card3, 
                    //                        card4 = @Card4, 
                    //                        card5 = @Card5, 
                    //                        card6 = @Card6, 
                    //                        card7 = @Card7, 
                    //                        card8 = @Card8 
                    //                    WHERE banner = @Banner";

                    //var updateResult = await _dbConnection.ExecuteAsync(updateQuery, new
                    //{
                    //    cms.banner,
                    //    cms.midcontentimage,
                    //    cms.headline,
                    //    cms.content,
                    //    cms.card1,
                    //    cms.card2,
                    //    cms.card3,
                    //    cms.card4,
                    //    cms.card5,
                    //    cms.card6,
                    //    cms.card7,
                    //    cms.card8
                    //});

                    //return updateResult > 0;
                    var updateResult = await _dbConnection.UpdateAsync(existingCms);
                    return updateResult;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
