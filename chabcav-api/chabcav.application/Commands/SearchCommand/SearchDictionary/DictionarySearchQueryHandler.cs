using chabcav.application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace chabcav.application.Queries.Dictionary
{
    public class DictionarySearchQueryHandler : IRequestHandler<DictionarySearchQuery, IEnumerable<DictionarySearchResultDto>>
    {
        private readonly IContentRepository _repository;

        public DictionarySearchQueryHandler(IContentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DictionarySearchResultDto>> Handle(DictionarySearchQuery request, CancellationToken cancellationToken)
        {
            return await _repository.SearchDictionaryAsync(request.Query);
        }
    }
}
