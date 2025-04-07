using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Queries.GetProfile
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Profile>
    {
        private readonly IProfileRepository _profileRepository;

        public GetProfileQueryHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public Task<Profile> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            return _profileRepository.GetProfileAsync(request.UserId);
        }
    }
}
