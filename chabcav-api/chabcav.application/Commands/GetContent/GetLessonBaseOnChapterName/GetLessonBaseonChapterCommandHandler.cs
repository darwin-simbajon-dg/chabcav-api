using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using chabcav.application.Interfaces;
using chabcav.domain.Entities;

public class GetLessonBaseOnChapterNameHandler : IRequestHandler<GetLessonBaseOnChapterNameCommand, List<AllLesson>>
{
    private readonly IContentRepository _contentRepository;

    public GetLessonBaseOnChapterNameHandler(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    public async Task<List<AllLesson>> Handle(GetLessonBaseOnChapterNameCommand request, CancellationToken cancellationToken)
    {
        return await _contentRepository.GetLessonsBySelectedChapterAsync(request.ChapterName);
    }
}
