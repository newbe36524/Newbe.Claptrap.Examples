using System.Threading.Tasks;
using HelloClaptrap.Actors.Sku.Events;
using HelloClaptrap.IActor;
using HelloClaptrap.Models;
using HelloClaptrap.Models.Sku;
using HelloClaptrap.Models.Sku.Events;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;

namespace HelloClaptrap.Actors.Sku
{
    [ClaptrapStateInitialFactoryHandler(typeof(SkuStateInitHandler))]
    [ClaptrapEventHandler(typeof(InventoryUpdateEventHandler), ClaptrapCodes.SkuInventoryUpdate)]
    public class SkuGrain : ClaptrapBoxGrain<SkuState>, ISkuGrain
    {
        public SkuGrain(IClaptrapGrainCommonService claptrapGrainCommonService)
            : base(claptrapGrainCommonService)
        {
        }

        public Task<int> GetInventoryAsync()
        {
            return Task.FromResult(StateData.Inventory);
        }

        public async Task<int> UpdateInventoryAsync(int diff)
        {
            if (diff == 0)
            {
                throw new BizException("diff can`t be 0");
            }

            var old = StateData.Inventory;
            var newInventory = old + diff;
            if (newInventory < 0)
            {
                throw new BizException(
                    $"failed to update inventory. It will be less than 0 if add diff amount. current : {old} , diff : {diff}");
            }

            var evt = this.CreateEvent(new InventoryUpdateEvent
            {
                Diff = diff,
                NewInventory = newInventory
            });
            await Claptrap.HandleEventAsync(evt);
            return StateData.Inventory;
        }
    }
}