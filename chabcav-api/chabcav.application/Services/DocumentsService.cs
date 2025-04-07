using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Services
{
    public interface IDocumentService
    {
        string ExtractTextFromDocx(byte[] fileData);
    }



}
