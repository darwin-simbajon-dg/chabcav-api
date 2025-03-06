using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.AddProfile
{
    public class AddProfileCommandHandler : IRequestHandler<AddProfileCommand, Guid>
    {
        private readonly IProfileRepository _profileRepository;

        public AddProfileCommandHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Guid> Handle(AddProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = new Profile
            {               
                fullname = request.Fullname,
                email = request.Email,
                phonenumber = request.Phonenumber,
                location = request.Location,
                information = request.Information,
                birthdate = request.Birthdate
            };

            var result = await _profileRepository.AddProfileAsync(profile);

            return result;

        }
    }
}
