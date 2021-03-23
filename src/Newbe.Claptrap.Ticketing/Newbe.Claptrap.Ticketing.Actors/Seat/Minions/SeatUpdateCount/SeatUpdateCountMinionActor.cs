using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors.Runtime;
using Newbe.Claptrap.Dapr;
using Newbe.Claptrap.Dapr.Core;
using Newbe.Claptrap.Ticketing.Actors.Seat.Minions.SeatUpdateCount.Events;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;

namespace Newbe.Claptrap.Ticketing.Actors.Seat.Minions.SeatUpdateCount
{
    [Actor(TypeName = ClaptrapCodes.SeatUpdateCountMinionActor)]
    [ClaptrapEventHandler(typeof(TakeSeatEventHandler), ClaptrapCodes.TakeSeatEvent)]
    public class SeatUpdateCountMinionActor : ClaptrapBoxActor<NoneStateData>, ISeatUpdateCountMinionActor
    {
        private readonly IEventSerializer<EventJsonModel> _eventSerializer;

        public SeatUpdateCountMinionActor(ActorHost actorHost,
            IClaptrapActorCommonService claptrapActorCommonService,
            IEventSerializer<EventJsonModel> eventSerializer) :
            base(actorHost, claptrapActorCommonService)
        {
            _eventSerializer = eventSerializer;
        }

        public async Task MasterEventReceivedAsync(IEnumerable<IEvent> events)
        {
            foreach (var evt in events)
            {
                await Claptrap.HandleEventAsync(evt);
            }
        }

        public async Task MasterEventReceivedJsonAsync(IEnumerable<EventJsonModel> events)
        {
            var items = events.Select(_eventSerializer.Deserialize);
            foreach (var @event in items)
            {
                await Claptrap.HandleEventAsync(@event);
            }
        }

        public Task WakeAsync()
        {
            return Task.CompletedTask;
        }
    }
}