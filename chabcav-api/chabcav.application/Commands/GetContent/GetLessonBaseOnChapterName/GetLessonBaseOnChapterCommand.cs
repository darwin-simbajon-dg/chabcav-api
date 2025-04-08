using chabcav.domain.Entities;
using MediatR;
using System.Collections.Generic;

public class GetLessonBaseOnChapterNameCommand : IRequest<List<AllLesson>>
{
    public string ChapterName { get; }

    public GetLessonBaseOnChapterNameCommand(string chapterName)
    {
        ChapterName = chapterName;
    }
}
