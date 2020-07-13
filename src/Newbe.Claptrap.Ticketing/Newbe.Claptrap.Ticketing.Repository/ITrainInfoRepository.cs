using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public interface ITrainInfoRepository
    {
        Task<int[]> GetStationsAsync(int trainId);
        Task<int[]> GetTrainsAsync(int locationId);
        Task<int[]> GetTrainsAsync(int fromLocationId, int toLocationId);
    }
}