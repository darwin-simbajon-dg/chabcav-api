using chabcav.application.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Queries.GetDashboard
{
    public class GetDashboardQuery : IRequest<DashboardData>
    {
    }
}
