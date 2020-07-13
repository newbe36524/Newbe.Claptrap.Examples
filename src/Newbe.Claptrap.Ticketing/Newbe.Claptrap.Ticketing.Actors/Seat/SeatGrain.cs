using System.Linq;
using System.Threading.Tasks;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Ticketing.Actors.Seat.Events;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;
using Orleans;

namespace Newbe.Claptrap.Ticketing.Actors.Seat
{
    [ClaptrapStateInitialFactoryHandler(typeof(SeatInfoInitHandler))]
    [ClaptrapEventHandler(typeof(TakeSeatEventHandler), ClaptrapCodes.TakeSeat)]
    public class SeatGrain : ClaptrapBoxGrain<SeatInfo>, ISeatGrain
    {
        private int _seatId;

        public SeatGrain(IClaptrapGrainCommonService claptrapGrainCommonService)
            : base(claptrapGrainCommonService)
        {
        }

        public int SeatId
        {
            get
            {
                if (_seatId == 0)
                {
                    _seatId = int.Parse(this.GetPrimaryKeyString());
                }

                return _seatId;
            }
            set => _seatId = value;
        }

        public Task TakeSeatAsync(int fromLocationId, int toLocationId, string requestId)
        {
            if (!StateData.LocationDic.TryGetValue(fromLocationId, out var fromIndex))
            {
                throw CreateNotFoundException();
            }

            if (!StateData.LocationDic.TryGetValue(toLocationId, out var toIndex))
            {
                throw CreateNotFoundException();
            }

            if (fromIndex >= toIndex)
            {
                throw CreateNotFoundException();
            }

            var distance = toIndex - fromIndex;
            var notRequested = StateData.RequestIds
                .Skip(fromIndex)
                .Take(distance)
                .All(string.IsNullOrEmpty);
            if (!notRequested)
            {
                throw new SeatHasBeenTakenException(SeatId,
                    fromLocationId,
                    toLocationId);
            }

            var evt = this.CreateEvent(new TakeSeatEvent
            {
                RequestId = requestId,
                FromLocationId = fromLocationId,
                ToLocationId = toLocationId
            });
            return Claptrap.HandleEventAsync(evt);

            LocationNotFoundException CreateNotFoundException()
            {
                return new LocationNotFoundException(SeatId,
                    fromLocationId,
                    toLocationId);
            }
        }
    }
}