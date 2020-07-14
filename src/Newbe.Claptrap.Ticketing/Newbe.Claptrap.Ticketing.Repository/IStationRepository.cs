using System.Collections.Generic;
using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public interface IStationRepository
    {
        Task<string> GetNameAsync(int stationId);
        Task<Dictionary<int, string>> GetNamesAsync(IEnumerable<int> stationId);
    }
}