using System.Collections.Generic;
using System.Threading.Tasks;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Ticketing.Actors.Seat.Minions.SeatUpdateCount.Events;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;

namespace Newbe.Claptrap.Ticketing.Actors.Seat.Minions.SeatUpdateCount
{
    [ClaptrapEventHandler(typeof(TakeSeatEventHandler), ClaptrapCodes.TakeSeatEvent)]
    public class SeatUpdateCountMinionGrain : ClaptrapBoxGrain<NoneStateData>, ISeatUpdateCountMinionGrain
    {
        public SeatUpdateCountMinionGrain(IClaptrapGrainCommonService claptrapGrainCommonService)
            : base(claptrapGrainCommonService)
        {
        }

        public async Task MasterEventReceivedAsync(IEnumerable<IEvent> events)
        {
            foreach (var evt in events)
            {
                await Claptrap.HandleEventAsync(evt);
            }
        }

        public Task WakeAsync()
        {
            return Task.CompletedTask;
        }
    }
}