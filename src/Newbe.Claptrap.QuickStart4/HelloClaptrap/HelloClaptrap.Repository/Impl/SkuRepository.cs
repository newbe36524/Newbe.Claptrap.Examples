using System.Threading.Tasks;
using HelloClaptrap.Models;

namespace HelloClaptrap.Repository.Impl
{
    public class SkuRepository : ISkuRepository
    {
        public Task<int> GetInitInventoryAsync(string skuId)
        {
            if (!skuId.Contains("-"))
            {
                throw new BizException($"sku not found with skuId : {skuId}");
            }

            var parts = skuId.Split("-");
            if (parts.Length != 2)
            {
                throw new BizException($"sku not found with skuId : {skuId}");
            }

            var brandCode = parts[0];
            switch (brandCode)
            {
                case "yueluo":
                    return Task.FromResult(666);
                case "robot":
                    return Task.FromResult(10);
                default:
                    throw new BizException($"sku not found with skuId : {skuId}");
            }
        }
    }
}