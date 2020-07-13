using System.Threading.Tasks;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.Ticketing.IActor
{
    public interface ITrainGran : IClaptrapGrain
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromLocationId"></param>
        /// <param name="toLocationId"></param>
        /// <returns></returns>
        Task<int> GetLeftSeatCountAsync(int fromLocationId, int toLocationId);
    }
}