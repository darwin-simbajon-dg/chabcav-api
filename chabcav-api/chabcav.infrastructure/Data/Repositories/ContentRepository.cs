using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.application.Interfaces;
using chabcav.domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace chabcav.infrastructure.Data.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public readonly IDbConnection _dbconnection;

        public ContentRepository(UserManager<IdentityUser> userManager, IDbConnection dbConnection)
        {
            _dbconnection = dbConnection;
            _userManager = userManager;
        }

        public async Task<bool> AddContentAsync(Chapter chapter, Lesson lesson)
        {
            try
            {
                var chapterResult = await _dbconnection.InsertAsync<Chapter>(chapter);
                lesson.chapterid = chapter.chapterid;
                var lessonResult = await _dbconnection.InsertAsync<Lesson>(lesson);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Lesson> GetLessonByIdAsync(Guid lessonId)
        {
           
                return await _dbconnection.GetAsync<Lesson>(lessonId);
       
        }

        public async Task<Lesson> GetLessonByIdAsync(Guid lessonid, bool useRawQuery = false)
        {
            if (useRawQuery)
            {
                string query = "SELECT * FROM Lessons WHERE lessonid = @LessonId";
                return await _dbconnection.QueryFirstOrDefaultAsync<Lesson>(query, new { LessonId = lessonid });
            } else if (!useRawQuery)
            {
                return await _dbconnection.GetAsync<Lesson>(lessonid);
            }

           return await _dbconnection.GetAsync<Lesson>(lessonid);
        }

        public async Task<Chapter> GetChapterByIdAsync(Guid chapterId)
        {
            return await _dbconnection.GetAsync<Chapter>(chapterId);
        }

        public async Task<bool> UpdateLessonAsync(Lesson lesson)
        {
            try
            {
                var result = await _dbconnection.UpdateAsync(lesson);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<bool> UpdateChapterAsync(Chapter chapter)
        {
            try
            {
                var result = await _dbconnection.UpdateAsync(chapter);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*public async Task<Lesson> GetLessonByIdAsync(Guid lessonId)
        {
            return await _dbconnection.GetAsync<Lesson>(lessonId);
            // string query = "SELECT * FROM Lessons WHERE lessonid = @LessonId";
            // return await _dbConnection.QueryFirstOrDefaultAsync<Lesson>(query, new { LessonId = lessonId });
        }*/

    }
}
