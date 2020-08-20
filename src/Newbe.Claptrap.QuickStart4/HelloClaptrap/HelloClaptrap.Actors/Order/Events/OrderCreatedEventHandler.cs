using System.Threading.Tasks;
using HelloClaptrap.Models.Order;
using HelloClaptrap.Models.Order.Events;
using Newbe.Claptrap;

namespace HelloClaptrap.Actors.Order.Events
{
    public class OrderCreatedEventHandler
        : NormalEventHandler<OrderState, OrderCreatedEvent>
    {
        public override ValueTask HandleEvent(OrderState stateData,
            OrderCreatedEvent eventData,
            IEventContext eventContext)
        {
            stateData.OrderCreated = true;
            return new ValueTask();
        }
    }
}