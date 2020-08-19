using System.Threading.Tasks;
using HelloClaptrap.Models;
using HelloClaptrap.Models.Order;
using HelloClaptrap.Models.Order.Events;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;

namespace HelloClaptrap.IActor
{
    [ClaptrapState(typeof(OrderState), ClaptrapCodes.OrderGrain)]
    [ClaptrapEvent(typeof(OrderCreatedEvent), ClaptrapCodes.OrderCreated)]
    public interface IOrderGrain : IClaptrapGrain
    {
        Task CreateOrderAsync(CreateOrderInput input);
    }
}