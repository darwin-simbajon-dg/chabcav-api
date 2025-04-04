using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Model
{
    public class DashboardData
    {
        public List<CountryData> CountryData { get; set; }

        public List<int> NoOfUsersInAWeek { get; set; }

        public List<int> MonthlyVisit { get; set; }

        public List<int> NoOfUsersThatCompletedPerMonth { get; set; }

        public int NoOfVisits { get; set; }

        public int TotalUsers { get; set; }

        public List<Coordinates> MapCoordinates { get; set; }
    }

    public class CountryData
    {
        public string Country { get; set; }
        public string Flag { get; set; }
        public int NoOfUsers { get; set; }
    }
}
