using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
    [Table("aspnetuserroles")]
    public class UserRole
    {
        public Guid userid { get; set; }
        public Guid roleid { get; set; }
    }
}
