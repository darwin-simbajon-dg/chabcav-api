using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.UpdateCMSContent
{
    public class UpdateCMSContentRequest
    {
        public string Content { get; set; }
        public string Headline { get; set; }

        // New properties added to the request
        public string Aboutus { get; set; }
        public string BannerContent { get; set; }
        public string BannerSecondContent { get; set; }
        public string CharacterReference { get; set; }
    }
}
