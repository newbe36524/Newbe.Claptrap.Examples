using System.Threading.Tasks;
using Newbe.Claptrap.IGrain.Domain.Sku.Events;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Shop.Models;

namespace Newbe.Claptrap.IGrain.Domain.Sku.Master
{
    [ClaptrapState(typeof(SkuStateData), ClaptrapCodes.Sku)]
    [ClaptrapEvent(typeof(SkuSoldEventData), ClaptrapCodes.SkuSoldEvent)]
    public interface ISkuGrain : IClaptrapGrain
    {
        Task<SoldResult> SoldAsync(string buyerUserId);
        Task SetupAsync();
        Task<SkuStatus> GetStatusAsync();
    }
}