using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Queries.GetCMS
{
    public class GetCMSQueryHandler : IRequestHandler<GetCMSQuery, CMS>
    {
        private readonly ICMSRepository _cmsRepository;

        public GetCMSQueryHandler(ICMSRepository cmsRepository)
        {
            _cmsRepository = cmsRepository;
        }

        public Task<CMS> Handle(GetCMSQuery request, CancellationToken cancellationToken)
        {
            return _cmsRepository.GetCMS();
        }
    }
}
