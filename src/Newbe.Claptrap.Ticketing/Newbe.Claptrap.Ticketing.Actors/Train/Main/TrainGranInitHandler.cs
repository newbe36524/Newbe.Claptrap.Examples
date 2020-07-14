using System.Threading.Tasks;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Repository;

namespace Newbe.Claptrap.Ticketing.Actors.Train.Main
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
            var re = TrainInfo.Create(stations, 10000);
            return re;
        }
    }
}