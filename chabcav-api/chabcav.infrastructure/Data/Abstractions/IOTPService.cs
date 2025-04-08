using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Abstractions
{
    public interface IOTPService
    {
        Task<string> GenerateOTP();
       
    }
}
