using System.Collections.Generic;
using System.Threading.Tasks;
using HelloClaptrap.Actors.DbGrains.Order.Events;
using HelloClaptrap.IActor;
using HelloClaptrap.Models;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;

namespace HelloClaptrap.Actors.DbGrains.Order
{
    [ClaptrapEventHandler(typeof(OrderCreatedEventHandler), ClaptrapCodes.OrderCreated)]
    public class OrderDbGrain : ClaptrapBoxGrain<NoneStateData>, IOrderDbGrain
    {
        public OrderDbGrain(IClaptrapGrainCommonService claptrapGrainCommonService)
            : base(claptrapGrainCommonService)
        {
        }

        public async Task MasterEventReceivedAsync(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
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