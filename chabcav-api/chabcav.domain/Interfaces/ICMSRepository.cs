using chabcav.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Interfaces
{
    public interface ICMSRepository
    {
        Configuration GetConfiguration();

        Task<int> CreateConfiguration(Configuration configuration);

        Task<bool> UpdateCMSImages(CMS cms);

        Task<CMS> GetCMS();
        Task<bool> UpdateContent(string content, string headline);
    }
}
