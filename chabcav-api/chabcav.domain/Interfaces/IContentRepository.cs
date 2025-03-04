using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.domain.Entities;

namespace chabcav.application.Interfaces
{
    public interface IContentRepository
    {
        Task<bool> AddContentAsync(Chapter chapter, Lesson lesson);

        //Updating Lesson
        Task<Lesson> GetLessonByIdAsync(Guid lessonId, bool useRawQuery = false); // pag get ng id
        Task<bool> UpdateLessonAsync(Lesson lesson); // update ng lesson edi wow

        //Updating Chapter
        Task<Chapter> GetChapterByIdAsync(Guid chapterId); // pag get ng id
        Task<bool> UpdateChapterAsync(Chapter chapter); // update ng chapter name
    }
}
