using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.domain.Entities;
using MediatR;

namespace chabcav.application.Commands.AddContent
{
    public class AddContentCommand: IRequest<bool>
    {
        public Chapter Chapter { get; set; }
        public Lesson Lesson { get; set; }
    }
}
