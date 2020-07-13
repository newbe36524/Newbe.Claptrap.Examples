using System.Threading.Tasks;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;

namespace Newbe.Claptrap.Ticketing.Actors.Seat.Events
{
    public class TakeSeatEventHandler
        : NormalEventHandler<SeatInfo, TakeSeatEvent>
    {
        public override ValueTask HandleEvent(SeatInfo stateData, TakeSeatEvent eventData, IEventContext eventContext)
        {
            var requestIds = stateData.RequestIds;
            var fromIndex = stateData.LocationDic[eventData.FromLocationId];
            var toIndex = stateData.LocationDic[eventData.ToLocationId];
            for (var i = fromIndex; i < toIndex; i++)
            {
                requestIds[i] = eventData.RequestId;
            }

            return new ValueTask();
        }
    }
}