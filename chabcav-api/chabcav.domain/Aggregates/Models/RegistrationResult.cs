using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Aggregates.Models
{
    public struct RegistrationResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
