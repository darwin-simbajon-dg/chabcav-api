using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<Guid>
    {
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Location { get; set; }
        public string Information { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
