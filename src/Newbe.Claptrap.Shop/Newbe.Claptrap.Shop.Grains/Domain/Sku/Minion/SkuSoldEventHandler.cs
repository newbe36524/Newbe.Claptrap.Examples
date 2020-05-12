using System.Threading.Tasks;
using Newbe.Claptrap.IGrain.Domain.Sku.Events;
using Newbe.Claptrap.IGrain.Domain.Sku.Master;
using Newbe.Claptrap.Shop.Repository;

namespace Newbe.Claptrap.Shop.Grains.Domain.Sku.Minion
{
    public class SkuSoldEventHandler : IEventHandler
    {
        private readonly ISkuRepository _skuRepository;

        public SkuSoldEventHandler(
            ISkuRepository skuRepository)
        {
            _skuRepository = skuRepository;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }

        public async Task<IState> HandleEvent(IEventContext eventContext)
        {
            var skuStateData = (SkuStateData) eventContext.State.Data;
            var skuSoldEventData = (SkuSoldEventData) eventContext.Event.Data;
            await _skuRepository.UpdateToSoldAsync(skuStateData.Id, skuSoldEventData.BuyerUserId);
            return eventContext.State;
        }
    }
}