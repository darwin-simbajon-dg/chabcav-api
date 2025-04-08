using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.UpdatePassword
{
    public class UpdatePasswordCommand : IRequest<bool>
    {
        
       

        public UpdatePasswordCommand(Guid userId,string currentPassword,string newPassword)
        {
            UserId = userId;

            CurrentPassword = currentPassword;

            NewPassword = newPassword;


        }

        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }


}
