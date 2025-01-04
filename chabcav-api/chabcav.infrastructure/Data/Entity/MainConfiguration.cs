using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Entity
{
    [Dapper.Contrib.Extensions.Table("mainconfiguration")]
    public class MainConfiguration
    {
        [ExplicitKey]
        public Guid id { get; set; }
        public string bannerimage { get; set; }

        public string backgroundimage { get; set; }

        public string content { get; set; }
    }
}
