using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Text.Json;
using Dapper.Contrib.Extensions;
using MediatR;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace chabcav.domain.Entities
{
    [Table("lessons")]
    public class Lesson
    {
        [ExplicitKey]
        public Guid lessonid { get; private set; }

        public Guid chapterid { get; set; }

        public string lessonname { get; set; }


        public string lessoncontent { get; set; }

        public Lesson()
        {

            if (lessonid == Guid.Empty)
            {
                lessonid = Guid.NewGuid();  // Ensure UUID is assigned for new objects
            }
        }

        //for adding new 
        public Lesson(string lessonName, string lessonContent)
         {
             lessonid = Guid.NewGuid();  // Generate new Lesson ID
             //chapterid = chapterId;  // Use existing Chapter ID
             lessonname = lessonName;
             lessoncontent = lessonContent;
         }

        public void UpdateLesson(string lessonName, string lessonContent)
        {
            lessonname = lessonName;
            lessoncontent = lessonContent;
        }

       /* public class GetAllLessonsQuery : IRequest<List<Lesson>>
        {
            public int UserId { get; set; }
        }*/

        /*public Lesson(string lessonType, string lessonName = "", string lessonContent = "")
        {
            if (lessonType == "Add")
            {
                lessonid = Guid.NewGuid(); // Always generate a new GUID
                lessonname = lessonName;
                lessoncontent = lessonContent;
            }
            else if (lessonType == "GetAll")
            {
                // No initialization needed, just an empty constructor
            }
        }*/

    }
}
