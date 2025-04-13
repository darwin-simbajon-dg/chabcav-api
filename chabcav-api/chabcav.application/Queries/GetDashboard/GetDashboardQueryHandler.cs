using chabcav.application.Model;
using chabcav.domain.Interfaces;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Queries.GetDashboard
{
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardData>
    {
        private List<Coordinates> _coordinates;
        private List<CountryFlag> _countryFlags;
        private readonly IAuditRepository _auditRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;

        public GetDashboardQueryHandler(IAuditRepository auditRepository, IUserRepository userRepository,
            IProfileRepository profileRepository)
        {
            _auditRepository = auditRepository;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }

        public async Task<DashboardData> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            _coordinates = GetCoordinates();
            _countryFlags = GetCountryFlags();

            var countryDataTask = GetCountryData();
            var weeklyVisitDataTask = GetWeeklyVisit();
            var monthlyVisitDataTask = GetMonthlyVisit();
            var monthlyCompletedUsersTask = GetMonthlyCompleted();
            var noOfVisitsTask = GetTotalVisits();
            var totalUsersTask = GetTotalUsers();

            await Task.WhenAll(countryDataTask, weeklyVisitDataTask, monthlyVisitDataTask, monthlyCompletedUsersTask, noOfVisitsTask, totalUsersTask);

            var countryData = await countryDataTask;
            var weeklyVisitData = await weeklyVisitDataTask;
            var monthlyVisitData = await monthlyVisitDataTask;
            var monthlyCompletedUsers = await monthlyCompletedUsersTask;
            var noOfVisits = await noOfVisitsTask;
            var totalUsers = await totalUsersTask;

            return new DashboardData
            {
                CountryData = countryData,
                NoOfUsersInAWeek = weeklyVisitData,
                MonthlyVisit = monthlyVisitData,
                NoOfUsersThatCompletedPerMonth = monthlyCompletedUsers,
                NoOfVisits = noOfVisits,
                TotalUsers = totalUsers,
                MapCoordinates = GetMapCoordinates(countryData, _coordinates)
            };
        }

        private async Task<List<CountryData>> GetCountryData()
        {
  
            var profiles = await _profileRepository.GetAllProfilesAsync();
 
            var usersPerLocation = profiles
                .GroupBy(p => p.location)
                .Select(g => new CountryData
                {
                    Country = g.Key,
                    Flag = GetCountryFlag(g.Key),
                    NoOfUsers = g.Count()
                })
                .OrderByDescending(cd => cd.NoOfUsers)
                .Take(5)
                .ToList();


            return usersPerLocation;

            //new List<CountryData>
            //        {
            //            new CountryData
            //            {
            //                Country = "Romania",
            //                Flag = GetCountryFlag("Romania"),
            //                NoOfUsers = 100
            //            },
            //            new CountryData
            //            {
            //                Country = "Philippines",
            //                Flag = GetCountryFlag("Philippines"),
            //                NoOfUsers = 50
            //            },
            //            new CountryData
            //            {
            //                Country = "Japan",
            //                Flag = GetCountryFlag("Japan"),
            //                NoOfUsers = 30
            //            },
            //            new CountryData
            //            {
            //                Country = "Australia",
            //                Flag = GetCountryFlag("Australia"),
            //                NoOfUsers = 20
            //            },
            //            new CountryData
            //            {
            //                Country = "Spain",
            //                Flag = GetCountryFlag("Spain"),
            //                NoOfUsers = 10
            //            }
            //        };
        }

        private async Task<int> GetTotalUsers()
        {
            var result = await _userRepository.GetAllAsync();

            var count = result.Count();

            return count;
        }

        private async Task<int> GetTotalVisits()
        {
            var result = await _auditRepository.GetAll();

            var count = result.Count(a => a.action == "LOGIN");

            return count;
        }

        private async Task<List<int>> GetMonthlyCompleted()
        {
            List<int> monthlyVisit = new List<int>();
            var alldata = await _auditRepository.GetAll();

            for (int i = 0; i < 12; i++)
            {
                var date = DateTime.Now.AddMonths(-i);
                var count = alldata.Count(a => a.actiondate.Month == date.Month && a.actiondate.Year == date.Year && a.action == "COMPLETED");
                monthlyVisit.Add(count);
            }

            return monthlyVisit;
        }

        private async Task<List<int>> GetMonthlyVisit()
        {
           List<int> monthlyVisit = new List<int>();
           var alldata = await _auditRepository.GetAll();
           
           for(int i = 0; i < 12; i++)
            {
                var date = DateTime.Now.AddMonths(-i);
                var count = alldata.Count(a => a.actiondate.Month == date.Month && a.actiondate.Year == date.Year && a.action == "LOGIN");
                monthlyVisit.Add(count);
            }

           return monthlyVisit;
        }

        private async Task<List<int>> GetWeeklyVisit()
        {
            List<int> weekData = new List<int>();
            var allData = await _auditRepository.GetAll();
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            for(int i = 0; i < 7; i++)
            {
                var date = DateTime.Now.AddDays(-i);
                var count = allData.Count(a => a.actiondate.Date == date.Date && a.action == "LOGIN");
                weekData.Add(count);
            }

            return weekData;
        }

        private string GetCountryFlag(string v)
        {
            return $"https://www.worldometers.info/{_countryFlags.FirstOrDefault(x => x.Name == v)?.Flag}";
        }

        private List<CountryFlag> GetCountryFlags()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot\\data", "countryflags.json");
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<CountryFlag>>(json);
        }

        private List<Coordinates> GetCoordinates()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot\\data", "countries.csv");
            var lines = File.ReadAllLines(filePath);
            var coordinatesList = new List<Coordinates>();

            foreach (var line in lines.Skip(1)) // Skip header line
            {
                var values = line.Split(',');
                var coordinates = new Coordinates
                {
                    Name = values[2],
                    Coords = new double[] { double.Parse(values[0]), double.Parse(values[1]) }
                };
                coordinatesList.Add(coordinates);
            }

            return coordinatesList;
        }

        private List<Coordinates> GetMapCoordinates(List<CountryData> countryData, List<Coordinates>? coordinates)
        {
           return coordinates.Where(x => countryData.Any(y => y.Country == x.Name)).ToList();
        }
    }
}
