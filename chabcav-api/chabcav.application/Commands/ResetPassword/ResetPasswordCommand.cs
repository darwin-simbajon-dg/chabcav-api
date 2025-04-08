using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<string>
    {
        public string EmailAddress { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Otp { get; set; }
    }
}
