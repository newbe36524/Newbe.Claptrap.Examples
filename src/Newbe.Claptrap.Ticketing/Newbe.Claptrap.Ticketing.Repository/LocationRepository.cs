using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public class LocationRepository : ILocationRepository
    {
        public Task<string> GetNameAsync(int locationId)
        {
            var locationName = DataSource.LocationNames[locationId];
            return Task.FromResult(locationName);
        }
    }
}