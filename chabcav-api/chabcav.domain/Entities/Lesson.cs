using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Dapper.Contrib.Extensions;
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

        public Lesson() { }

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

 
    }
}
