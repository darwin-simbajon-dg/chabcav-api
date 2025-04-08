using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
