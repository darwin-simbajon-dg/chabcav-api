using chabcav.domain.Interfaces;
using chabcav.infrastructure.Data.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.UpdateProfileImage
{
    public class UploadProfileImageCommandHandler : IRequestHandler<UpdateProfileImageCommand, bool>
    {
        private readonly IProfileRepository _profileRepository;
  
        public UploadProfileImageCommandHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<bool> Handle(UpdateProfileImageCommand request, CancellationToken cancellationToken)
        {
            //var imageId = await _googleDriveUploader.UploadFileToGoogleDrive(request.Image);

            //if(imageId == null)
            //{
            //    return string.Empty;
            //}

            try
            {

               var FirebaseStorage = new FirebaseStorageService();
                await FirebaseStorage.UploadProfileImage(request.Image);
               /* var path = Path.Combine("wwwroot/uploads", request.Image.FileName);
               using var stream = new FileStream(path, FileMode.Create);
                await request.Image.CopyToAsync(stream);*/


                var isUpdated = await _profileRepository.UpdateProfileImage(request.UserId, request.Image.FileName);
                return isUpdated;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
