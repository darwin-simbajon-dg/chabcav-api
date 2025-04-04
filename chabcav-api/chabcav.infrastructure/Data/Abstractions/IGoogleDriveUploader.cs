using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Abstractions
{
    public interface IGoogleDriveUploader
    {
       Task<string> UploadFileToGoogleDrive(IFormFile file);

    }
}
