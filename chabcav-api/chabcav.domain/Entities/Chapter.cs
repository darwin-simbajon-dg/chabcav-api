using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace chabcav.domain.Entities
{
    [Table("chapters")]
    /*public class Chapter
    {
        //public Guid chapterid { get; set; }
        //public string chaptername { get; set; }

        //public Guid ChapterId { get; private set; }
        //public string ChapterName { get; private set; }
        [ExplicitKey]
        [Column("chapterid")] 
        public Guid chapterid { get; private set; }

        [Column("chaptername")]
        public string chaptername { get; set; }

        public Chapter(string chapterName)
        {
            chapterid = Guid.NewGuid();
            chaptername = chapterName;
        }

        public Guid GetChapterId()
        {
            return chapterid;
        }
    }*/
    public class Chapter
    {
        [ExplicitKey]
        [Column("chapterid")]
        public Guid chapterid { get; private set; }

        [Column("chaptername")]
        public string chaptername { get; set; }

        public Chapter() { }

        public Chapter(string chapterName)
        {
            chapterid = Guid.NewGuid();  // Pre-generate UUID before insert
            chaptername = chapterName;
        }

    

        public void UpdateChapter(string chapterName)
        {
            chaptername = chapterName;
        }

    }
}
