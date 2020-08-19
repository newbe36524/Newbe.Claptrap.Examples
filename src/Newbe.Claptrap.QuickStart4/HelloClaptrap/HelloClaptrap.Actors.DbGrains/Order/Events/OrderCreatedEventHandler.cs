using System.Threading.Tasks;
using HelloClaptrap.Models.Order.Events;
using HelloClaptrap.Repository;
using Newbe.Claptrap;
using Newtonsoft.Json;

namespace HelloClaptrap.Actors.DbGrains.Order.Events
{
    public class OrderCreatedEventHandler
        : NormalEventHandler<NoneStateData, OrderCreatedEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderCreatedEventHandler(
            IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public override async ValueTask HandleEvent(NoneStateData stateData,
            OrderCreatedEvent eventData,
            IEventContext eventContext)
        {
            var orderId = eventContext.State.Identity.Id;
            await _orderRepository.SaveAsync(eventData.UserId, orderId, JsonConvert.SerializeObject(eventData.Skus));
        }
    }
}