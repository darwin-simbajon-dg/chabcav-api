using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using chabcav.infrastructure.Data.Connections;
using chabcav.infrastructure.Data.Entity;
using Dapper;
using Dapper.Contrib.Extensions;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using chabcav.application.Commands.CMS;

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

        /*public async Task<CMS> GetCMS()
        {
            try
            {
                var result = await _dbConnection.GetAllAsync<CMS>();

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }*/

        public async Task<CMS> GetCMS()
        {
            //var db = FirebaseInitializer.Initialize();
            //DocumentReference docRef = db.Collection("cms").Document("main");
            //DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

           /* if (snapshot.Exists)
            {
                var cms = snapshot.ConvertTo<CMS>();

                // Check and assign Firebase Storage URLs if image fields are not null or empty
                if (!string.IsNullOrEmpty(cms.banner))
                {
                    cms.banner = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.banner}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.midcontentimage))
                {
                    cms.midcontentimage = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.midcontentimage}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.card1))
                {
                    cms.card1 = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.card1}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.card2))
                {
                    cms.card2 = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.card2}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.card3))
                {
                    cms.card3 = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.card3}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.card4))
                {
                    cms.card4 = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.card4}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.card5))
                {
                    cms.card5 = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.card5}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.card6))
                {
                    cms.card6 = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.card6}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.card7))
                {
                    cms.card7 = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.card7}?alt=media";
                }
                if (!string.IsNullOrEmpty(cms.card8))
                {
                    cms.card8 = $"https://firebasestorage.googleapis.com/v0/b/chabcav-a62d8.appspot.com/o/{cms.card8}?alt=media";
                }

                return cms;
            }*/

            return null;
        }


        /*public async Task<CMS> GetCMS()
        {
            var db = FirebaseInitializer.Initialize();
            DocumentReference docRef = db.Collection("cms").Document("main");
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<CMS>();
            }

            return null;
        }*/


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

        /*public async Task<bool> UpdateCMSImages(CMS cms)
        {
            try
            {
                var existingCms = await _dbConnection.QueryFirstOrDefaultAsync<CMS>("SELECT * FROM cms");

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
                    if (cms.banner != null) {
                        existingCms.banner = cms.banner;
                    }

                    if (cms.midcontentimage != null)
                    {
                        existingCms.midcontentimage = cms.midcontentimage;
                    }                

                    if (cms.card1 != null)
                    {
                        existingCms.card1 = cms.card1;
                    }

                    if (cms.card2 != null)
                    {
                        existingCms.card2 = cms.card2;
                    }

                    if (cms.card3 != null)
                    {
                        existingCms.card3 = cms.card3;
                    }

                    if (cms.card4 != null)
                    {
                        existingCms.card4 = cms.card4;
                    }

                    if (cms.card5 != null)
                    {
                        existingCms.card5 = cms.card5;
                    }

                    if (cms.card6 != null)
                    {
                        existingCms.card6 = cms.card6;
                    }

                    if (cms.card7 != null)
                    {
                        existingCms.card7 = cms.card7;
                    }

                    if (cms.card8 != null)
                    {
                        existingCms.card8 = cms.card8;
                    }

                    //return updateResult > 0;
                    var updateResult = await _dbConnection.UpdateAsync(existingCms);
                    return updateResult;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }*/

        /* public async Task<bool> UpdateCMSImages(CMS cms)
         {
             var db = FirebaseInitializer.Initialize();
             DocumentReference docRef = db.Collection("cms").Document("main");

             Dictionary<string, object> updates = new();

             if (!string.IsNullOrEmpty(cms.banner)) updates["banner"] = cms.banner;
             if (!string.IsNullOrEmpty(cms.midcontentimage)) updates["midcontentimage"] = cms.midcontentimage;
             if (!string.IsNullOrEmpty(cms.card1)) updates["card1"] = cms.card1;
             if (!string.IsNullOrEmpty(cms.card2)) updates["card2"] = cms.card2;
             if (!string.IsNullOrEmpty(cms.card3)) updates["card3"] = cms.card3;
             if (!string.IsNullOrEmpty(cms.card4)) updates["card4"] = cms.card4;
             if (!string.IsNullOrEmpty(cms.card5)) updates["card5"] = cms.card5;
             if (!string.IsNullOrEmpty(cms.card6)) updates["card6"] = cms.card6;
             if (!string.IsNullOrEmpty(cms.card7)) updates["card7"] = cms.card7;
             if (!string.IsNullOrEmpty(cms.card8)) updates["card8"] = cms.card8;

             await docRef.SetAsync(updates, SetOptions.MergeAll);
             return true;
         }*/

        public async Task<bool> UpdateCMSImages(CMS cms)
        {
            //var db = FirebaseInitializer.Initialize();
            //DocumentReference docRef = db.Collection("cms").Document("main");

            //Dictionary<string, object> updates = new();

            // Check if the image fields are not null or empty, and add to the updates dictionary
         /*   if (!string.IsNullOrEmpty(cms.banner)) updates["banner"] = cms.banner;
            if (!string.IsNullOrEmpty(cms.midcontentimage)) updates["midcontentimage"] = cms.midcontentimage;
            if (!string.IsNullOrEmpty(cms.card1)) updates["card1"] = cms.card1;
            if (!string.IsNullOrEmpty(cms.card2)) updates["card2"] = cms.card2;
            if (!string.IsNullOrEmpty(cms.card3)) updates["card3"] = cms.card3;
            if (!string.IsNullOrEmpty(cms.card4)) updates["card4"] = cms.card4;
            if (!string.IsNullOrEmpty(cms.card5)) updates["card5"] = cms.card5;
            if (!string.IsNullOrEmpty(cms.card6)) updates["card6"] = cms.card6;
            if (!string.IsNullOrEmpty(cms.card7)) updates["card7"] = cms.card7;
            if (!string.IsNullOrEmpty(cms.card8)) updates["card8"] = cms.card8;

            try
            {
                // Use UpdateAsync for a partial update (if you're updating specific fields)
                await docRef.UpdateAsync(updates);
                return true;
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.Error.WriteLine($"Failed to update CMS images: {ex.Message}");
                return false;  // Return false in case of an error
            }*/
            return true;
        }


        /*public async Task<bool> UpdateContent(string content, string headline)
        {
            try
            {
                var result = await _dbConnection.ExecuteAsync("UPDATE cms SET content = @Content, headline = @Headline", new { Content = content, Headline = headline });

                return result > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }*/

        public async Task<bool> UpdateContent(string content, string headline)
        {
            //var db = FirebaseInitializer.Initialize();
    //       // DocumentReference docRef = db.Collection("cms").Document("main");

    //        var updates = new Dictionary<string, object>
    //{
    //    { "content", content },
    //    { "headline", headline }
    //};

    //        await docRef.UpdateAsync(updates);
            return true;
        }


    }
}
