using chabcav.domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<Profile>
    {
        public Guid UserId { get; set; }
    }
}
