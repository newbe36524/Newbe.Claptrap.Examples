using System.Threading.Tasks;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;

namespace Newbe.Claptrap.Ticketing.IActor
{
    [ClaptrapState(typeof(SeatInfo), ClaptrapCodes.SeatGrain)]
    [ClaptrapEvent(typeof(TakeSeatEvent), ClaptrapCodes.TakeSeat)]
    public interface ISeatGrain : IClaptrapGrain
    {
        /// <summary>
        /// Take a seat
        /// </summary>
        /// <param name="fromLocationId"></param>
        /// <param name="toLocationId"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        /// <exception cref="LocationNotFoundException">location not found</exception>
        /// <exception cref="SeatHasBeenTakenException">seat has been taken</exception>
        Task TakeSeatAsync(int fromLocationId, int toLocationId, string requestId);
    }
}