using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.AddConfiguration
{
    public class AddConfigurationCommand : IRequest<int>
    {
        public string BannerImage { get; set; }

        public string BackgroundImage { get; set; }

        public string Content { get; set; }
    }
}
