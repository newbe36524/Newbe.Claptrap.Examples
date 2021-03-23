using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors.Runtime;
using Newbe.Claptrap.Dapr;
using Newbe.Claptrap.Ticketing.Actors.Seat.Main.Events;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;

namespace Newbe.Claptrap.Ticketing.Actors.Seat.Main
{
    [Actor(TypeName = ClaptrapCodes.SeatActor)]
    [ClaptrapStateInitialFactoryHandler(typeof(SeatInfoInitHandler))]
    [ClaptrapEventHandler(typeof(TakeSeatEventHandler), ClaptrapCodes.TakeSeatEvent)]
    public class SeatActor : ClaptrapBoxActor<SeatInfo>, ISeatActor
    {
        private readonly ActorHost _actorHost;
        private int _seatId;

        public SeatActor(ActorHost actorHost,
            IClaptrapActorCommonService claptrapActorCommonService)
            : base(actorHost, claptrapActorCommonService)
        {
            _actorHost = actorHost;
        }


        public int SeatId
        {
            get
            {
                if (_seatId == 0)
                {
                    _seatId = int.Parse(_actorHost.Id.GetId());
                }

                return _seatId;
            }
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