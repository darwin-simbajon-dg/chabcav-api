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
        }


        public string Headline { get; set; }
        public string Content { get; set; }
    }

}
