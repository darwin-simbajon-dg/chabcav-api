using Dapper.Contrib.Extensions;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
    [Table("cms")]
     public class CMS
     {
         [Key]
         public int id { get; set; }
         public string banner { get; set; }
         public string midcontentimage { get; set; }
         public string headline { get; set; }
         public string content { get; set; }
         public string card1 { get; set; }
         public string card2 { get; set; }
         public string card3 { get; set; }
         public string card4 { get; set; }
         public string card5 { get; set; }
         public string card6 { get; set; }
         public string card7 { get; set; }
         public string card8 { get; set; }





     }

    

}
