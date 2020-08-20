using HelloClaptrap.Models;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;

namespace HelloClaptrap.IActor
{
    [ClaptrapMinion(ClaptrapCodes.OrderGrain)]
    [ClaptrapState(typeof(NoneStateData), ClaptrapCodes.OrderDbGrain)]
    public interface IOrderDbGrain : IClaptrapMinionGrain
    {
    }
}