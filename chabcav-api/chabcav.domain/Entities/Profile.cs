using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
    [Table("profiles")]
    public class Profile
    {
        public Guid id { get; set; }
        public string information { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
        public string location { get; set; }
        public DateTime birthdate { get; set; }
    }
}
