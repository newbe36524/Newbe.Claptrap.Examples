using System.Threading.Tasks;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Repository;

namespace Newbe.Claptrap.Ticketing.Actors.Seat.Main
{
    public class SeatInfoInitHandler : IInitialStateDataFactory
    {
        private readonly ITrainInfoRepository _trainInfoRepository;

        public SeatInfoInitHandler(
            ITrainInfoRepository trainInfoRepository)
        {
            _trainInfoRepository = trainInfoRepository;
        }

        public async Task<IStateData> Create(IClaptrapIdentity identity)
        {
            var seatId = SeatId.FromSeatId(identity.Id);
            var stations = await _trainInfoRepository.GetStationsAsync(seatId.TrainId);
            var seatInfo = SeatInfo.Create(stations);
            return seatInfo;
        }
    }
}