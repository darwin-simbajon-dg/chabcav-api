using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using chabcav.application.Interfaces;
using chabcav.domain.Entities;
using MediatR;

namespace chabcav.application.Queries.GetAllLessons
{
    public class GetAllLessonsQueryHandler : IRequestHandler<GetAllLessonsQuery, List<AllLesson>>
    {
        private readonly IContentRepository _contentRepository;

        public GetAllLessonsQueryHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<List<AllLesson>> Handle(GetAllLessonsQuery request, CancellationToken cancellationToken)
        {
          
            return await _contentRepository.GetAllLessonsAsync();
        }
    }
}
