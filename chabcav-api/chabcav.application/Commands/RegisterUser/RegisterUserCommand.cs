using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Guid>
    {
        public string Username { get; }
        public string Email { get; }
        public string Password { get; }

        public RegisterUserCommand(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}
