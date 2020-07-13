using System.Collections.Generic;
using System.Threading.Tasks;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Repository;

namespace Newbe.Claptrap.Ticketing.Actors.Train
{
    public class TrainGranInitHandler : IInitialStateDataFactory
    {
        private readonly ITrainInfoRepository _trainInfoRepository;

        public TrainGranInitHandler(
            ITrainInfoRepository trainInfoRepository)
        {
            _trainInfoRepository = trainInfoRepository;
        }

        public async Task<IStateData> Create(IClaptrapIdentity identity)
        {
            var trainId = int.Parse(identity.Id);
            var stations = await _trainInfoRepository.GetStationsAsync(trainId);
            const int leftCount = 10000;
            var dic = new Dictionary<StationTuple, int>(StationTuple.FromLocationIdToLocationIdComparer);
            for (var i = 0; i < stations.Length; i++)
            {
                for (var j = i; j < stations.Length; j++)
                {
                    var stationTuple = new StationTuple
                    {
                        FromStationId = stations[i],
                        ToStationId = stations[j]
                    };
                    dic[stationTuple] = leftCount;
                }
            }

            var re = new TrainInfo
            {
                Stations = stations,
                SeatCount = dic
            };
            return re;
        }
    }
}