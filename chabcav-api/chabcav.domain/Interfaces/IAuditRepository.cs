using chabcav.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Interfaces
{
    public interface IAuditRepository
    {
        Task LogActivity(Activity activity);

        Task<IEnumerable<Activity>> GetAll();
    }
}
