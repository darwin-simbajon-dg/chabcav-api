using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.domain.Entities;

namespace chabcav.domain.Interfaces
{
    public interface IUsersProgressRepository
    {
        //Task<int> CreateAsync(UsersProgress progress);
        
        Task<bool> AddUserProgressAsync(UsersProgress userProgress);
        Task<List<string>> GetCompletedChaptersByUserIdAsync(string userId);



    }

}
