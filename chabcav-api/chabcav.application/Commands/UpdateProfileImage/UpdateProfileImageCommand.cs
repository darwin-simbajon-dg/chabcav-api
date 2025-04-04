using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.UpdateProfileImage
{
    public class UpdateProfileImageCommand : IRequest<bool>
    {
        public UpdateProfileImageCommand(IFormFile formData, Guid userId)
        {
            Image = formData;
            UserId = userId;
        }

        public IFormFile Image { get; set; }

        public Guid UserId { get; set; }
    }
}
