using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
    [Table("aspnetroles")]
    public class Role
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string normalizedname { get; set; }
    }
}
