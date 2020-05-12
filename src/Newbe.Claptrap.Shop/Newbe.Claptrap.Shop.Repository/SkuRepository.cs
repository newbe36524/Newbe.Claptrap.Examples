using System.Collections.Generic;
using System.Threading.Tasks;
using Newbe.Claptrap.Shop.Models;

namespace Newbe.Claptrap.Shop.Repository
{
    public class SkuRepository : ISkuRepository
    {
        public Task UpdateToSoldAsync(string id, string buyerUserId)
        {
            throw new System.NotImplementedException();
        }

        public Task<SkuStatus> GetSkuStatusAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> GetSkuIdsAsync(int pageIndex, int pageSize)
        {
            throw new System.NotImplementedException();
        }
    }
}