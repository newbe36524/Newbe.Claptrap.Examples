using System.Linq;
using System.Threading.Tasks;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Ticketing.Actors.Seat.Main.Events;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;
using Orleans;

namespace Newbe.Claptrap.Ticketing.Actors.Seat.Main
{
    [ClaptrapStateInitialFactoryHandler(typeof(SeatInfoInitHandler))]
    [ClaptrapEventHandler(typeof(TakeSeatEventHandler), ClaptrapCodes.TakeSeatEvent)]
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

        public Task TakeSeatAsync(int fromStationId, int toStationId, string requestId)
        {
            if (!StateData.StationDic.TryGetValue(fromStationId, out var fromIndex))
            {
                throw CreateNotFoundException();
            }

            if (!StateData.StationDic.TryGetValue(toStationId, out var toIndex))
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
                    fromStationId,
                    toStationId);
            }

            var evt = this.CreateEvent(new TakeSeatEvent
            {
                RequestId = requestId,
                FromStationId = fromStationId,
                ToStationId = toStationId
            });
            return Claptrap.HandleEventAsync(evt);

            StationNotFoundException CreateNotFoundException()
            {
                var seatId = Main.SeatId.FromSeatId(SeatId);
                return new StationNotFoundException(seatId.TrainId,
                    fromStationId,
                    toStationId);
            }
        }
    }
}