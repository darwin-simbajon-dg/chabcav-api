using System;
using System.Data;
using System.Threading.Tasks;
using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using Dapper;

namespace chabcav.infrastructure.Data.Repositories
{
    public class UsersProgressRepository : IUsersProgressRepository
    {
        private readonly IDbConnection _dbConnection;

        public UsersProgressRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> AddUserProgressAsync(UsersProgress userProgress)
        {
            try
            {
                var sql = @"
                    INSERT INTO usersprogress (userid, chaptername, datecompleted, status)
                    VALUES (@UserId, @ChapterName, @DateCompleted, @Status);
                ";

                var result = await _dbConnection.ExecuteAsync(sql, new
                {
                    userProgress.UserId,
                    userProgress.ChapterName,
                    DateCompleted = DateTime.UtcNow,
                    Status = "Chapter Completed"
                });

                return result > 0;
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return false;
            }
        }

        public async Task<List<string>> GetCompletedChaptersByUserIdAsync(string userId)
        {
            //using var connection = _context.CreateConnection();
            var sql = @"
        SELECT DISTINCT ChapterName
        FROM usersprogress
        WHERE UserId = @UserId AND Status = 'Chapter Completed'";

            var result = await _dbConnection.QueryAsync<string>(sql, new { UserId = userId });
            return result.ToList();
        }

       
        

       
    }
}
