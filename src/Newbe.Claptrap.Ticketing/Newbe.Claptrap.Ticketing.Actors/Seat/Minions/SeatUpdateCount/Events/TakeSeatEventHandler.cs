using System.Threading.Tasks;
using Newbe.Claptrap.Ticketing.Actors.Seat.Main;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;
using Orleans;

namespace Newbe.Claptrap.Ticketing.Actors.Seat.Minions.SeatUpdateCount.Events
{
    public class TakeSeatEventHandler : NormalEventHandler<NoneStateData, TakeSeatEvent>
    {
        private readonly IGrainFactory _grainFactory;

        public TakeSeatEventHandler(
            IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        public override async ValueTask HandleEvent(NoneStateData stateData,
            TakeSeatEvent eventData,
            IEventContext eventContext)
        {
            var claptrapIdentity = eventContext.State.Identity;
            var seatId = SeatId.FromSeatId(claptrapIdentity.Id);
            var trainGran = _grainFactory.GetGrain<ITrainGran>(seatId.TrainId.ToString());
            await trainGran.UpdateCountAsync(eventData.FromStationId, eventData.ToStationId);
        }
    }
}