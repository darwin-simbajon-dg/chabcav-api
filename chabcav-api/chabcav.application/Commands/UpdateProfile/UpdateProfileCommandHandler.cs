using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Guid>
    {
        private readonly IProfileRepository _profileRepository;

        public UpdateProfileCommandHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public async Task<Guid> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = new Profile
            {
                id = Guid.Parse(request.UserId),
                fullname = request.Fullname,
                email = request.Email,
                phonenumber = request.Phonenumber,
                location = request.Location,
                information = request.Information,
                birthdate = request.Birthdate
            };

            var updateProfile = await _profileRepository.UpdateProfileAsync(profile);

            return profile.id;
        }
    }
}
