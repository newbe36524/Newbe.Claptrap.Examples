using System.Threading.Tasks;
using Newbe.Claptrap.IGrain.Domain.Sku.Master;
using Newbe.Claptrap.Shop.Models;

namespace Newbe.Claptrap.Shop.Grains.Domain.Sku.Master
{
    public class SkuSoldEventHandler : IEventHandler
    {
        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }

        public Task<IState> HandleEvent(IEventContext eventContext)
        {
            var skuStateData = (SkuStateData) eventContext.State.Data;
            skuStateData.Status = SkuStatus.Sold;
            return Task.FromResult(eventContext.State);
        }
    }
}