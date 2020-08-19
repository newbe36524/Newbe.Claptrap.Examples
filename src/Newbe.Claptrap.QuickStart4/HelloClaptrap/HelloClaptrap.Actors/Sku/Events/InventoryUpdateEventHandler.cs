using System.Threading.Tasks;
using HelloClaptrap.Models.Sku;
using HelloClaptrap.Models.Sku.Events;
using Newbe.Claptrap;

namespace HelloClaptrap.Actors.Sku.Events
{
    public class InventoryUpdateEventHandler
        : NormalEventHandler<SkuState, InventoryUpdateEvent>
    {
        public override ValueTask HandleEvent(SkuState stateData,
            InventoryUpdateEvent eventData,
            IEventContext eventContext)
        {
            stateData.Inventory = eventData.NewInventory;
            return new ValueTask();
        }
    }
}