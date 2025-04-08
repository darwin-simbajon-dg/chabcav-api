using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.application.Queries.Dictionary;
using chabcav.domain.Entities;

namespace chabcav.application.Interfaces
{
    public interface IContentRepository
    {
        // Adding Content
        Task<bool> AddContentAsync(Chapter chapter, Lesson lesson);

        //Updating Lesson
        Task<Lesson> GetLessonByIdAsync(Guid lessonId, bool useRawQuery = false); // pag get ng id
        Task<bool> UpdateLessonAsync(Lesson lesson); // update ng lesson edi wow

        //Updating Chapter
        Task<Chapter> GetChapterByIdAsync(Guid chapterId); // pag get ng id
        Task<bool> UpdateChapterAsync(Chapter chapter); // update ng chapter name

        //geting all lessons
        Task<List<AllLesson>> GetAllLessonsAsync();

        //geting lessons by chapter
        Task<List<AllLesson>> GetLessonsBySelectedChapterAsync(string chapterName);

        //Searching on Dictionary
        Task<IEnumerable<DictionarySearchResultDto>> SearchDictionaryAsync(string query);

        //Uploading Dictionary File
        Task<bool> UploadDictionaryFileAsync(UploadDictionaryFile file);

        //This is for getting the dictionary file by id, this for hide for meantime and for improvment
        Task<UploadDictionaryFile> GetDictionaryFileByIdAsync(int id);
        Task<IEnumerable<UploadDictionaryFile>> GetAllDictionaryFilesAsync();

        Task AddUserProgressAsync(UserProgress progress);

    }
}
