using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.UpdateCMSContent
{
    public class UpdateCMSContentCommandHandler : IRequestHandler<UpdateCMSContentCommand, bool>
   {
        private readonly ICMSRepository _cmsRepository;

        public UpdateCMSContentCommandHandler(ICMSRepository cmsRepository)
        {
            _cmsRepository = cmsRepository; 
        }

        public async Task<bool> Handle(UpdateCMSContentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _cmsRepository.UpdateContent(request.Content, request.Headline);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
