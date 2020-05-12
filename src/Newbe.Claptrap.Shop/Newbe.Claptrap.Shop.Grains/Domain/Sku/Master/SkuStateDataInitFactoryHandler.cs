using System.Threading.Tasks;
using Newbe.Claptrap.IGrain;
using Newbe.Claptrap.IGrain.Domain.Sku.Master;
using Newbe.Claptrap.Shop.Repository;

namespace Newbe.Claptrap.Shop.Grains.Domain.Sku.Master
{
    public class SkuStateDataInitFactoryHandler : IInitialStateDataFactory
    {
        private readonly ISkuRepository _skuRepository;

        public SkuStateDataInitFactoryHandler(
            ISkuRepository skuRepository)
        {
            _skuRepository = skuRepository;
        }

        public async Task<IStateData> Create(IClaptrapIdentity identity)
        {
            var skuId = identity.Id;
            var skuStatus = await _skuRepository.GetSkuStatusAsync(skuId);
            var re = new SkuStateData
            {
                Id = skuId,
                Status = skuStatus
            };
            return re;
        }
    }
}