using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public interface ILocationRepository
    {
        Task<string> GetNameAsync(int locationId);
    }
}