using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace chabcav.application.Commands.UpdateChapter
{
    public class UpdateChapterCommand : IRequest<bool>
    {
        public Guid ChapterId { get; set; }
        public string ChapterName { get; set; }

        public UpdateChapterCommand(Guid chapterId, string chapterName)
        {
            ChapterId = chapterId;
            ChapterName = chapterName;
        }   

    }
}
