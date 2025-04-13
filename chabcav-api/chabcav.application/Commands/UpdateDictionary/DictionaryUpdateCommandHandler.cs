using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.application.Interfaces;
using MediatR;

namespace chabcav.application.Commands.UpdateDictionary
{
    public class DictionaryUpdateCommandHandler : IRequestHandler<DictionaryUpdateCommand, bool>
    {
        private readonly IContentRepository _repository;

        public DictionaryUpdateCommandHandler(IContentRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DictionaryUpdateCommand request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateDictionaryHtmlAsync(request.UpdatedHtml);
        }
    }

}
