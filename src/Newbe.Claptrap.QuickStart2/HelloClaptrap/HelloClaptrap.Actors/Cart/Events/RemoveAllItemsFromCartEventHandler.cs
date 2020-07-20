using System.Threading.Tasks;
using HelloClaptrap.Models.Cart;
using HelloClaptrap.Models.Cart.Events;
using Newbe.Claptrap;

namespace HelloClaptrap.Actors.Cart.Events
{
    public class RemoveAllItemsFromCartEventHandler
        : NormalEventHandler<CartState, RemoveAllItemsFromCartEvent>
    {
        public override ValueTask HandleEvent(CartState stateData,
            RemoveAllItemsFromCartEvent eventData,
            IEventContext eventContext)
        {
            stateData.Items = null;
            return new ValueTask();
        }
    }
}