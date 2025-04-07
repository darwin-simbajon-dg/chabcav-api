using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace chabcav.application.Commands.AddContent.UploadFileDictionary
{
    public class UploadDictionaryFileCommand : IRequest<bool>
    {
        public string FileName { get; set; }  // File name
        public byte[] FileContent { get; set; }  // File content as a byte array
        public IFormFile File { get; set; }  // IFormFile for any additional processing
    }

}
