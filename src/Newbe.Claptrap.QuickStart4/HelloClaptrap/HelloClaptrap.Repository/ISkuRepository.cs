using System.Threading.Tasks;

namespace HelloClaptrap.Repository
{
    public interface ISkuRepository
    {
        Task<int> GetInitInventoryAsync(string skuId);
    }
}