using System.Threading.Tasks;
using Dapr.Actors.Client;
using Newbe.Claptrap.Dapr;
using Newbe.Claptrap.Ticketing.Actors.Seat.Main;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;

namespace Newbe.Claptrap.Ticketing.Actors.Seat.Minions.SeatUpdateCount.Events
{
    public class TakeSeatEventHandler : NormalEventHandler<NoneStateData, TakeSeatEvent>
    {
        private readonly IActorProxyFactory _actorProxyFactory;

        public TakeSeatEventHandler(
            IActorProxyFactory actorProxyFactory)
        {
            _actorProxyFactory = actorProxyFactory;
        }

        public override async ValueTask HandleEvent(NoneStateData stateData,
            TakeSeatEvent eventData,
            IEventContext eventContext)
        {
            var claptrapIdentity = eventContext.State.Identity;
            var seatId = SeatId.FromSeatId(claptrapIdentity.Id);
            var trainActor = _actorProxyFactory.GetClaptrap<ITrainGran>(seatId.TrainId.ToString());
            await trainActor.UpdateCountAsync(eventData.FromStationId, eventData.ToStationId);
        }
    }
}