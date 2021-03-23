using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors.Runtime;
using Newbe.Claptrap.Dapr;
using Newbe.Claptrap.Ticketing.Actors.Train.Main.Events;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Models.Train.Events;

namespace Newbe.Claptrap.Ticketing.Actors.Train.Main
{
    [Actor(TypeName = ClaptrapCodes.TrainActor)]
    [ClaptrapStateInitialFactoryHandler(typeof(TrainGranInitHandler))]
    [ClaptrapEventHandler(typeof(UpdateCountEventHandler), ClaptrapCodes.UpdateCount)]
    public class TrainGran : ClaptrapBoxActor<TrainInfo>, ITrainGran
    {
        private readonly ActorHost _actorHost;

        public TrainGran(ActorHost actorHost,
            IClaptrapActorCommonService claptrapActorCommonService)
            : base(actorHost, claptrapActorCommonService)
        {
            _actorHost = actorHost;
        }

        private int _trainId;

        public int TrainId
        {
            get
            {
                if (_trainId == 0)
                {
                    _trainId = int.Parse(_actorHost.Id.GetId());
                }

                return _trainId;
            }
        }


        public Task<int> GetLeftSeatCountAsync(int fromStationId, int toStationId)
        {
            if (!StateData.TryGetSeatCount(fromStationId, toStationId, out var count))
            {
                throw new StationNotFoundException(TrainId, fromStationId, toStationId);
            }

            return Task.FromResult(count);
        }

        public Task<Dictionary<int, IDictionary<int, int>>> GetAllCountAsync()
        {
            return Task.FromResult(StateData.SeatCount.ToDictionary(x => x.Key, x => x.Value));
        }

        public Task UpdateCountAsync(int fromStationId, int toStationId)
        {
            var stations = StateData.Stations;
            var matchFrom = false;
            var matchTo = false;
            foreach (var station in stations)
            {
                if (station == fromStationId)
                {
                    matchFrom = true;
                }

                if (matchFrom && station == toStationId)
                {
                    matchTo = true;
                    break;
                }
            }

            if (!matchTo)
            {
                throw new StationNotFoundException(TrainId, fromStationId, toStationId);
            }

            var evt = this.CreateEvent(new UpdateCountEvent
            {
                FromStationId = fromStationId,
                ToStationId = toStationId
            });
            return Claptrap.HandleEventAsync(evt);
        }
    }
}