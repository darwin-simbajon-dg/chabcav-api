using System.Collections.Generic;
using MediatR;
using chabcav.domain.Entities;

namespace chabcav.application.Queries.GetAllLessons
{
    public class GetAllLessonsQuery : IRequest<List<AllLesson>>
    {
        // No parameters needed since we fetch all lessons
    }
}
