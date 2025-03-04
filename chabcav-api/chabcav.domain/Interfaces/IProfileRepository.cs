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
        Task<bool> UpdateProfileAsync(Profile profile);
    }
}
