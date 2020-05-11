using System.Threading.Tasks;
using Newbe.Claptrap.IGrain;

namespace Newbe.Claptrap.Shop.Grains
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