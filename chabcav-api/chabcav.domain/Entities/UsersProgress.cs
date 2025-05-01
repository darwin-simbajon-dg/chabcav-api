using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
    [Table("usersprogress")]
    public class UsersProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ChapterName { get; set; }
        public DateTime DateCompleted { get; set; }
        public string Status { get; set; }
        
    }
}

