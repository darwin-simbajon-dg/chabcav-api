using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.application.Commands.AddContent;
using chabcav.application.Interfaces;
using MediatR;

namespace chabcav.application.Commands.UpdateLesson
{
    public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand, bool>
    {
        private readonly IContentRepository _contentRepository;

        public UpdateLessonCommandHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<bool> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = await _contentRepository.GetLessonByIdAsync(request.LessonId);
            if (lesson == null) return false; // Lesson not found

            lesson.UpdateLesson(request.LessonName, request.LessonContent); // Update properties

            await _contentRepository.UpdateLessonAsync(lesson);
            return true; // Update successful
        }
    }
}
