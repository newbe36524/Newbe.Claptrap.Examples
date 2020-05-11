using System.Threading.Tasks;
using Newbe.Claptrap.IGrain;

namespace Newbe.Claptrap.Shop.Repository
{
    public interface ISkuRepository
    {
        Task UpdateSkuStatusAsync(string id, SkuStatus status);
        Task<SkuStatus> GetSkuStatusAsync(string id);
    }
}