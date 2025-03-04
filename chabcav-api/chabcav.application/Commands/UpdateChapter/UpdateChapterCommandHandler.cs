using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.application.Commands.UpdateLesson;
using chabcav.application.Interfaces;
using MediatR;

namespace chabcav.application.Commands.UpdateChapter
{
    public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, bool>
    {
        private readonly IContentRepository _contentRepository;

        public UpdateChapterCommandHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<bool> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _contentRepository.GetChapterByIdAsync(request.ChapterId);
            if (chapter == null) return false;

            chapter.UpdateChapter(request.ChapterName); // Update properties

            await _contentRepository.UpdateChapterAsync(chapter);
            return true; // Update successful
        }
    }
}
