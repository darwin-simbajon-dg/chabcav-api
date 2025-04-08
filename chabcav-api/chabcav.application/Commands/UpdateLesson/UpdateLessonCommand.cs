using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace chabcav.application.Commands.UpdateLesson
{
    public class UpdateLessonCommand : IRequest<bool>
    {
        public Guid LessonId { get; set; }
        public string LessonName { get; set; }
        public string LessonContent { get; set; }

        public UpdateLessonCommand(Guid lessonId, string lessonName, string lessonContent)
        {
            LessonId = lessonId;
            LessonName = lessonName;
            LessonContent = lessonContent;
        }
    }
}
