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
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;

        public GetProfileQueryHandler(IProfileRepository profileRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }

        public async Task<Profile> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {

            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(x => x.Id == request.UserId);

            var profile = await _profileRepository.GetProfileAsync(user.Id);
            return profile;
        }
    }
}
