using System.Threading.Tasks;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.IGrain
{
    [ClaptrapState(typeof(SkuStateData), ClaptrapCodes.Sku)]
    [ClaptrapEvent(typeof(SkuSoldEvent), ClaptrapCodes.SkuSoldEvent)]
    public interface ISkuGrain : IClaptrapGrain
    {
        Task<SoldResult> SoldAsync(string buyerUserId);
    }
}