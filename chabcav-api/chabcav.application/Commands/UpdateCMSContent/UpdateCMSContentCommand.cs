using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.UpdateCMSContent
{
    public class UpdateCMSContentCommand : IRequest<bool>
    {
        public UpdateCMSContentCommand(UpdateCMSContentRequest request)
        {
            Content = request.Content;
            Headline = request.Headline;
            // Initialize new properties
            Aboutus = request.Aboutus;
            BannerContent = request.BannerContent;
            BannerSecondContent = request.BannerSecondContent;
            CharacterReference = request.CharacterReference;
        }


        public string Headline { get; set; }
        public string Content { get; set; }

        // New properties added to the command
        public string Aboutus { get; set; }
        public string BannerContent { get; set; }
        public string BannerSecondContent { get; set; }
        public string CharacterReference { get; set; }
    }

}
