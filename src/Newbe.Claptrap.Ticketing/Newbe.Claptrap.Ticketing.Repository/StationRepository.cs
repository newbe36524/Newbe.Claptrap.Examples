using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public class StationRepository : IStationRepository
    {
        public Task<string> GetNameAsync(int stationId)
        {
            var re = DataSource.StationNames[stationId];
            return Task.FromResult(re);
        }

        public Task<Dictionary<int, string>> GetNamesAsync(IEnumerable<int> stationId)
        {
            var re = stationId.Distinct()
                .ToDictionary(x => x, x => DataSource.StationNames[x]);
            return Task.FromResult(re);
        }
    }
}