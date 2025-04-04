using chabcav.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> GetProfileAsync(Guid userId);
        Task<Profile> GetProfileByEmailAsync(string email);
        Task<Guid> UpdateProfileAsync(Profile profile);
        Task<Guid> AddProfileAsync(Profile profile);
        Task<bool> UpdateProfileImage(Guid userId, string imageId);
        Task<IEnumerable<Profile>> GetAllProfilesAsync();


    }
}
