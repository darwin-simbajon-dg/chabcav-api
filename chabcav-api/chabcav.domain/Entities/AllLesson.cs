using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace chabcav.domain.Entities
{
    [Table("lessons")]
    public class AllLesson
    {
        public Guid lessonid { get; private set; }

        public Guid chapterid { get; set; }

        public string lessonname { get; set; }
        public string chaptername { get; set; }


        public string lessoncontent { get; set; }
        public AllLesson()
        {
            
        }

        public class GetAllLessonsQuery : IRequest<List<AllLesson>>
        {
            public int UserId { get; set; }
        }
    }
}
