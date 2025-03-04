using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.domain.Entities;
using MediatR;

namespace chabcav.application.Commands.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<Profile>
    {
        public Guid UserId { get; set; }
    }
}
