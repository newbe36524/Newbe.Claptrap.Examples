using System.Threading.Tasks;
using HelloClaptrap.Models.Sku;
using HelloClaptrap.Repository;
using Newbe.Claptrap;

namespace HelloClaptrap.Actors.Sku
{
    public class SkuStateInitHandler : IInitialStateDataFactory
    {
        private readonly ISkuRepository _skuRepository;

        public SkuStateInitHandler(
            ISkuRepository skuRepository)
        {
            _skuRepository = skuRepository;
        }

        public async Task<IStateData> Create(IClaptrapIdentity identity)
        {
            var skuId = identity.Id;
            var inventory = await _skuRepository.GetInitInventoryAsync(skuId);
            var re = new SkuState
            {
                Inventory = inventory
            };
            return re;
        }
    }
}