using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.CMS
{
    public class CMSCommand : IRequest<bool>
    {
        public CMSCommand(CMSDto dto, List<IFormFile> files)
        {
            
            Data = dto;

            Files = files;
        }
        //public List<CmsImageUpload> CmsImageUploads { get; set; }

        public CMSDto Data { get; set; }

        public List<IFormFile> Files { get; set; }



    }
    
}

