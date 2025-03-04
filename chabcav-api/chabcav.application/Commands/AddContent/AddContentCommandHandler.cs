using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.application.Interfaces;
using MediatR;

namespace chabcav.application.Commands.AddContent
{
    public class AddContentCommandHandler : IRequestHandler<AddContentCommand, bool>
    {
        private readonly IContentRepository _contentRepository;
        public AddContentCommandHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;

        }
        public async Task<bool> Handle(AddContentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _contentRepository.AddContentAsync(request.Chapter, request.Lesson);
                return result;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
