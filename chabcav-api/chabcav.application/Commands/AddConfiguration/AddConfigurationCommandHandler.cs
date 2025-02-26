using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.AddConfiguration
{
    public class AddConfigurationCommandHandler : IRequestHandler<AddConfigurationCommand, int>
    {
        private readonly ICMSRepository _cmsRepository;

        public AddConfigurationCommandHandler(ICMSRepository cmsRepository)
        {
            _cmsRepository = cmsRepository;
        }

        public async Task<int> Handle(AddConfigurationCommand request, CancellationToken cancellationToken)
        {
            var configuration = new Configuration()
            {
                BannerImage = request.BannerImage,
                BackgroundImage = request.BackgroundImage,
                Content = request.Content
            };

            var response = await _cmsRepository.CreateConfiguration(configuration);

            return response;
        }
    }
}
