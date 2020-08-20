using System.Threading.Tasks;
using HelloClaptrap.Models;
using HelloClaptrap.Models.Sku;
using HelloClaptrap.Models.Sku.Events;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;

namespace HelloClaptrap.IActor
{
    [ClaptrapState(typeof(SkuState), ClaptrapCodes.SkuGrain)]
    [ClaptrapEvent(typeof(InventoryUpdateEvent), ClaptrapCodes.SkuInventoryUpdate)]
    public interface ISkuGrain : IClaptrapGrain
    {
        /// <summary>
        /// Get latest inventory of this sku
        /// </summary>
        /// <returns></returns>
        Task<int> GetInventoryAsync();

        /// <summary>
        /// Update inventory by add diff, diff could be negative number
        /// </summary>
        /// <param name="diff"></param>
        /// <returns>Inventory after updating</returns>
        Task<int> UpdateInventoryAsync(int diff);
    }
}