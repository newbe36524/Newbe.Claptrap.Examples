using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Ticketing.Actors.Train.Main.Events;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Models.Train.Events;
using Orleans;

namespace Newbe.Claptrap.Ticketing.Actors.Train.Main
{
    [ClaptrapStateInitialFactoryHandler(typeof(TrainGranInitHandler))]
    [ClaptrapEventHandler(typeof(UpdateCountEventHandler), ClaptrapCodes.UpdateCount)]
    public class TrainGran : ClaptrapBoxGrain<TrainInfo>, ITrainGran
    {
        public TrainGran(IClaptrapGrainCommonService claptrapGrainCommonService)
            : base(claptrapGrainCommonService)
        {
        }

        private int _trainId;

        public int TrainId
        {
            get
            {
                if (_trainId == 0)
                {
                    _trainId = int.Parse(this.GetPrimaryKeyString());
                }

                return _trainId;
            }
            set => _trainId = value;
        }


        public Task<int> GetLeftSeatCountAsync(int fromStationId, int toStationId)
        {
            if (!StateData.TryGetSeatCount(fromStationId, toStationId, out var count))
            {
                throw new StationNotFoundException(TrainId, fromStationId, toStationId);
            }

            return Task.FromResult(count);
        }

        public Task<Dictionary<StationTuple, int>> GetAllCountAsync()
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