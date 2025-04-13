using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.application.Interfaces;
using chabcav.application.Queries.Dictionary;
using chabcav.domain.Entities;
using chabcav.infrastructure.Data.Abstractions;
using chabcav.infrastructure.Data.Repositories;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace chabcav.infrastructure.Data.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public readonly IDbConnection _dbconnection;
        // Rest of the code remains unchanged

        public ContentRepository(UserManager<IdentityUser> userManager, IDbConnection dbConnection) // Modify constructor
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

        /*public async Task<IEnumerable<DictionarySearchResultDto>> SearchDictionaryAsync(string query)
        {
            var sql = @"
            SELECT 
            id,
            file_name AS FileName, 
            uploaded_at AS UploadedAt,
            ts_headline('english', extracted_text, plainto_tsquery('english', @Query)) AS MatchSnippet
            FROM dictionary_files
            WHERE to_tsvector('english', extracted_text) @@ plainto_tsquery('english', @Query);
            ";

            return await _dbconnection.QueryAsync<DictionarySearchResultDto>(sql, new { Query = query });
        }*/

        public async Task<IEnumerable<DictionarySearchResultDto>> SearchDictionaryAsync(string query)
        {
            var sql = @"
            SELECT 
                id,
                file_name AS FileName, 
                uploaded_at AS UploadedAt,
                ts_headline('english', extracted_text, plainto_tsquery('english', @Query)) AS MatchSnippet
            FROM dictionary_files
            WHERE uploaded_at = (
                SELECT MAX(uploaded_at) FROM dictionary_files
            )
            AND to_tsvector('english', extracted_text) @@ plainto_tsquery('english', @Query);
            ";

            return await _dbconnection.QueryAsync<DictionarySearchResultDto>(sql, new { Query = query });
        }

        public async Task<UploadDictionaryFile> GetDictionaryFileByIdAsync(int id)
        {
            var query = "SELECT * FROM dictionary_files WHERE id = @Id";
            return await _dbconnection.QueryFirstOrDefaultAsync<UploadDictionaryFile>(query, new { Id = id });
        }

        public async Task<IEnumerable<UploadDictionaryFile>> GetAllDictionaryFilesAsync()
         {
            var query = "SELECT * FROM dictionary_files";
             return await _dbconnection.QueryAsync<UploadDictionaryFile>(query);
         }

        public async Task<bool> UploadDictionaryFileAsync(UploadDictionaryFile file)
        {
           var sql = @"
            INSERT INTO dictionary_files (file_name, file_data, extracted_text, extracted_html, uploaded_at)
            VALUES (@FileName, @FileData, @ExtractedText, @ExtractedHtml, @UploadedAt)";


            var result = await _dbconnection.ExecuteAsync(sql, new
            {
                file.FileName,
                file.FileData,
                file.ExtractedText,
                file.ExtractedHtml, /// 👈 this was missing
                file.UploadedAt
              // 👈 this was added
            });

            return result > 0;
        }

        public async Task<bool> UpdateDictionaryHtmlAsync(string updatedHtml)
        {
            const string sql = @"
        UPDATE dictionary_files 
        SET extracted_html = @HtmlContent 
        WHERE id = (
            SELECT id 
            FROM dictionary_files 
            ORDER BY uploaded_at DESC 
            LIMIT 1
        )";

            var result = await _dbconnection.ExecuteAsync(sql, new { HtmlContent = updatedHtml });
            return result > 0;
        }

        public async Task<string> GetLatestDictionaryTextAsync()
        {
            const string sql = @"
        SELECT extracted_text
        FROM dictionary_files 
        ORDER BY id DESC 
        LIMIT 1";

            return await _dbconnection.QueryFirstOrDefaultAsync<string>(sql);
        }


        public async Task<string> GetLatestDictionaryHtmlAsync()
        {
            const string sql = @"
        SELECT extracted_html, extracted_text
        FROM dictionary_files 
        ORDER BY id DESC 
        LIMIT 1";

            return await _dbconnection.QueryFirstOrDefaultAsync<string>(sql);
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
                //string query = "SELECT * FROM Lessons WHERE lessonid IS NOT NULL";
                return await _dbconnection.QueryFirstOrDefaultAsync<Lesson>(query, new { LessonId = lessonid });
            }
            else if (!useRawQuery)
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

        public async Task<List<AllLesson>> GetAllLessonsAsync()
        {
            string query = @"
        SELECT 
            l.LessonId, 
            l.LessonName, 
            l.LessonContent, 
            c.ChapterName 
        FROM Lessons l
        INNER JOIN Chapters c ON l.ChapterId = c.ChapterId
        WHERE c.ChapterId IS NOT NULL ORDER BY c.ChapterName ASC;";

            var lessons = await _dbconnection.QueryAsync<AllLesson>(query);
            return lessons.ToList();
        }


        public async Task<List<AllLesson>> GetLessonsBySelectedChapterAsync(string chapterName)
        {
            string query = @"
        SELECT 
            l.LessonId, 
            l.LessonName, 
            l.LessonContent, 
            c.ChapterName 
        FROM Lessons l
        INNER JOIN Chapters c ON l.ChapterId = c.ChapterId
        WHERE c.ChapterName = @ChapterName";

            return (await _dbconnection.QueryAsync<AllLesson>(query, new { ChapterName = chapterName })).ToList();
        }


    //    public async Task AddUserProgressAsync(UserProgress progress)
    //    {
    //        var query = @"
    //    INSERT INTO usersprogress (usersid, usersname, chaptername, status, datecompleted)
    //    VALUES (@UsersId, @UsersName, @ChapterName, @Status, @DateCompleted)
    //";

    //        var parameters = new
    //        {
    //            UsersId = progress.UsersId,
    //            UsersName = progress.UsersName,
    //            ChapterName = progress.ChapterName,
    //            Status = progress.Status,
    //            DateCompleted = progress.DateCompleted
    //        };

    //        await _dbconnection.ExecuteAsync(query, parameters);
    //    }


    }
}
