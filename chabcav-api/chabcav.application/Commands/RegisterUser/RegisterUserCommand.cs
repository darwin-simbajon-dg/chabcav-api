using chabcav.domain.Aggregates.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<RegistrationResult>
    {
        public string Username { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; set; }


        public RegisterUserCommand(string username, string email, string password, string role)
        {
            Username = username;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}
