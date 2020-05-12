using System.Threading.Tasks;
using Newbe.Claptrap.IGrain;
using Newbe.Claptrap.IGrain.Domain.Sku.Minion;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.Shop.Grains.Domain.Sku.Minion
{
    [ClaptrapStateInitialFactoryHandler]
    [ClaptrapEventHandler(typeof(SkuSoldEventHandler), ClaptrapCodes.SkuSoldEvent)]
    public class SkuDbMinionGrain : ClaptrapBoxGrain<NoneStateData>, ISkuDbMinionGrain
    {
        public SkuDbMinionGrain(IClaptrapGrainCommonService claptrapGrainCommonService) : base(
            claptrapGrainCommonService)
        {
        }

        public Task MasterEventReceivedAsync(IEvent @event)
        {
            return Claptrap.HandleEventAsync(@event);
        }

        public Task WakeAsync()
        {
            return Task.CompletedTask;
        }
    }
}