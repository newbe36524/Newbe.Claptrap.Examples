using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Ticketing.Models;

namespace Newbe.Claptrap.Ticketing.IActor
{
    [ClaptrapMinion(ClaptrapCodes.SeatGrain)]
    [ClaptrapState(typeof(NoneStateData), ClaptrapCodes.SeatUpdateCountMinionGrain)]
    public interface ISeatUpdateCountMinionGrain : IClaptrapMinionGrain
    {
    }
}