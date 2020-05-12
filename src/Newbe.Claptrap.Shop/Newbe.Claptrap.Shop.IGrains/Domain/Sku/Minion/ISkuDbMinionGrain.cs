using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.IGrain.Domain.Sku.Minion
{
    [ClaptrapState(typeof(NoneStateData), ClaptrapCodes.SkuDb)]
    public interface ISkuDbMinionGrain : IClaptrapMinionGrain
    {
    }
}