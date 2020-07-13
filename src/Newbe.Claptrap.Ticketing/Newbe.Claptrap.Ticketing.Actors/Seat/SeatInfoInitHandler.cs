using System.Threading.Tasks;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Repository;

namespace Newbe.Claptrap.Ticketing.Actors.Seat
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
            var locations = await _trainInfoRepository.GetStationsAsync(seatId.TrainId);
            var seatInfo = SeatInfo.Create(locations);
            return seatInfo;
        }
    }
}