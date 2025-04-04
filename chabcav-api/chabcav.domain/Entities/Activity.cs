using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
    [Table("audit")]
    public class Activity
    {
        [Key]
        public Guid auditId { get; set; }
        public string action { get; set; }
        public Guid userid { get; set; }
        public DateTime actiondate { get; set; }
    }
}
