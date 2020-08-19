using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloClaptrap.Actors.Cart.Events;
using HelloClaptrap.IActor;
using HelloClaptrap.Models;
using HelloClaptrap.Models.Cart;
using HelloClaptrap.Models.Cart.Events;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;

namespace HelloClaptrap.Actors.Cart
{
    [ClaptrapEventHandler(typeof(AddItemToCartEventHandler), ClaptrapCodes.AddItemToCart)]
    [ClaptrapEventHandler(typeof(RemoveItemFromCartEventHandler), ClaptrapCodes.RemoveItemFromCart)]
    [ClaptrapEventHandler(typeof(RemoveAllItemsFromCartEventHandler), ClaptrapCodes.RemoveAllItemsFromCart)]
    public class CartGrain : ClaptrapBoxGrain<CartState>, ICartGrain
    {
        public CartGrain(
            IClaptrapGrainCommonService claptrapGrainCommonService)
            : base(claptrapGrainCommonService)
        {
        }

        public async Task<Dictionary<string, int>> AddItemAsync(string skuId, int count)
        {
            var evt = this.CreateEvent(new AddItemToCartEvent
            {
                Count = count,
                SkuId = skuId,
            });
            await Claptrap.HandleEventAsync(evt);
            return StateData.Items;
        }

        public async Task<Dictionary<string, int>> RemoveItemAsync(string skuId, int count)
        {
            var evt = this.CreateEvent(new RemoveItemFromCartEvent
            {
                Count = count,
                SkuId = skuId
            });
            await Claptrap.HandleEventAsync(evt);
            return StateData.Items;
        }

        public Task<Dictionary<string, int>> GetItemsAsync()
        {
            var re = StateData.Items ?? new Dictionary<string, int>();
            return Task.FromResult(re);
        }

        public Task RemoveAllItemsAsync()
        {
            if (StateData.Items?.Any() != true)
            {
                return Task.CompletedTask;
            }

            var removeAllItemsFromCartEvent = new RemoveAllItemsFromCartEvent();
            var evt = this.CreateEvent(removeAllItemsFromCartEvent);
            return Claptrap.HandleEventAsync(evt);
        }
    }
}