using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.domain.Entities;
using MediatR;

namespace chabcav.application.Commands.GetContent.GetLesson
{
    public class GetLessonCommand : IRequest<Lesson>
    {
        public Guid LessonId { get; set; }
        public GetLessonCommand(Guid lessonId)
        {
            LessonId = lessonId;
        }
    }
}
