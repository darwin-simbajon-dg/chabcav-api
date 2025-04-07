using MediatR;
using System.Collections.Generic;

namespace chabcav.application.Queries.Dictionary
{
    public class DictionarySearchQuery : IRequest<IEnumerable<DictionarySearchResultDto>>
    {
        public string Query { get; set; }
    }
}
