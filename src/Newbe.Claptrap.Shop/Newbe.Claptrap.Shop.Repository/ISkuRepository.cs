using System.Collections.Generic;
using System.Threading.Tasks;
using Newbe.Claptrap.Shop.Models;

namespace Newbe.Claptrap.Shop.Repository
{
    public interface ISkuRepository
    {
        Task UpdateToSoldAsync(string id, string buyerUserId);
        Task<SkuStatus> GetSkuStatusAsync(string id);
        Task<IEnumerable<string>> GetSkuIdsAsync(int pageIndex, int pageSize);
    }
}