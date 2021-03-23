using Newbe.Claptrap.Dapr.Core;
using Newbe.Claptrap.Ticketing.Models;

namespace Newbe.Claptrap.Ticketing.IActor
{
    [ClaptrapMinion(ClaptrapCodes.SeatActor)]
    [ClaptrapState(typeof(NoneStateData), ClaptrapCodes.SeatUpdateCountMinionActor)]
    public interface ISeatUpdateCountMinionActor : IClaptrapMinionActor
    {
    }
}