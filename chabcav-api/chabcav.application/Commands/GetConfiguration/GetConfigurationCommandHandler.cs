using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.GetConfiguration
{
    public class GetConfigurationCommandHandler : IRequestHandler<GetConfigurationCommand, ConfigurationResponse>
    {
        private readonly ICMSRepository _cmsRepository;

        public GetConfigurationCommandHandler(ICMSRepository cmsRepository)
        {
            _cmsRepository = cmsRepository;
        }

        public  Task<ConfigurationResponse> Handle(GetConfigurationCommand request, CancellationToken cancellationToken)
        {
            var response =   _cmsRepository.GetConfiguration();

            return Task.FromResult(new ConfigurationResponse()
            {
                BannerImage = response.BannerImage,
                BackgroundImage = response.BackgroundImage,
                Content = response.Content
            });
        }
    }
}
