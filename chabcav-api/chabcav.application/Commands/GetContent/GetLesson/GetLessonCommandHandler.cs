using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.application.Interfaces;
using chabcav.domain.Entities;
using MediatR;

namespace chabcav.application.Commands.GetContent.GetLesson
{
    public class GetLessonCommandHandler : IRequestHandler<GetLessonCommand, Lesson>
    {
        private readonly IContentRepository _contentRepository;

        public GetLessonCommandHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<Lesson> Handle(GetLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = await _contentRepository.GetLessonByIdAsync(request.LessonId);
            return lesson;
        }
    }
}
