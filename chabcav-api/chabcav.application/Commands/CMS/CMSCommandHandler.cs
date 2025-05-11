using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using chabcav.infrastructure.Data.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace chabcav.application.Commands.CMS
{
    public class CMSCommandHandler : IRequestHandler<CMSCommand, bool>
    {
        private readonly ICMSRepository _cmsRepository;

        public CMSCommandHandler(ICMSRepository cmsRepository )
        {
            _cmsRepository = cmsRepository;

        }



        public async Task<bool> Handle(CMSCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Declare variable for list of IFormFiles
                //Pass the IFormFiles from cmsImageUploads using Linq
                //var files = request.CmsImageUploads.Select(x => x.Image).ToList();

                var cms = new chabcav.domain.Entities.CMS()
                {
                    banner = request.Data.banner,
                    midcontentimage = request.Data.midcontentimage,
                    headline = request.Data.headline,
                    content = request.Data.content,
                    card1 = request.Data.card1,
                    card2 = request.Data.card2,
                    card3 = request.Data.card3,
                    card4 = request.Data.card4,
                    card5 = request.Data.card5,
                    card6 = request.Data.card6,
                    card7 = request.Data.card7,
                    card8 = request.Data.card8,

                    //newly added fields
                    aboutus = request.Data.aboutus,
                    bannercontent = request.Data.bannercontent,
                    bannersecondcontent = request.Data.bannersecondcontent,
                    characterreference = request.Data.characterreference
                };
                
              var result = _cmsRepository.UpdateCMSImages(cms);

                var FirebaseStorage = new FirebaseStorageService();
                await FirebaseStorage.UploadFilesAsync(request.Files);

                /* foreach (var formFile in request.Files)
                 {
                     if (formFile.Length > 0)
                     {
                         var filePath = Path.Combine("wwwroot\\uploads", formFile.FileName);
                         using (var stream = new FileStream(filePath, FileMode.Create))
                         {
                             await formFile.CopyToAsync(stream);
                         }
                     }
                 }*/

                //Upload files to Firebase Storage



                //Create variable for cms record and initialize new Cms

                //var cms = new domain.Entities.CMS
                //{
                //    banner = request.CmsImageUploads[0].Name,


                //};




                return true;


            }
            catch (Exception ex)
            {
                // Handle errors
                return false;
            }
        }
    }



}

