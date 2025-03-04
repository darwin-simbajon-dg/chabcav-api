using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
	public class Profile
	{
		public Guid Id { get; set; }
		public string Information { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
    }
}
