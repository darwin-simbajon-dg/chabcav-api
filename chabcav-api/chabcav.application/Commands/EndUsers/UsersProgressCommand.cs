using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace chabcav.application.Commands.EndUsers
{
    public class UsersProgressCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string ChapterName { get; set; }
    }


}
