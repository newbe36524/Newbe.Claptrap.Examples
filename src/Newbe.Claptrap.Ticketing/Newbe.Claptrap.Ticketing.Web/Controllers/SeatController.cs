using System;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors.Client;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Dapr;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Repository;
using Newbe.Claptrap.Ticketing.Web.Models;

namespace Newbe.Claptrap.Ticketing.Web.Controllers
{
    /// <summary>
    /// Seat Api
    /// </summary>
    [Route("api/[controller]")]
    public class SeatController : Controller
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly IStationRepository _stationRepository;
        private readonly ITrainInfoRepository _trainInfoRepository;

        public SeatController(
            IActorProxyFactory actorProxyFactory,
            IStationRepository stationRepository,
            ITrainInfoRepository trainInfoRepository)
        {
            _actorProxyFactory = actorProxyFactory;
            _stationRepository = stationRepository;
            _trainInfoRepository = trainInfoRepository;
        }

        /// <summary>
        /// Take a seat
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BlazorJsonResponse> TakeSeatAsync([FromBody] TakeSeatInput input)
        {
            var seatId = input.SeatId;
            if (string.IsNullOrEmpty(input.SeatId))
            {
                var random = new Random();
                var seatNumber = random.Next(0, 10000);
                seatId = $"{input.TrainId}{seatNumber:0000}";
            }

            var cartActor = _actorProxyFactory.GetClaptrap<ISeatActor>(seatId);
            var requestId = Guid.NewGuid().ToString("N");
            try
            {
                await cartActor.TakeSeatAsync(input.FromStationId, input.ToStationId, requestId);
            }
            catch (Exception e)
            {
                return new BlazorJsonResponse {Status = "0", Message = e.Message};
            }

            var fromName = await _stationRepository.GetNameAsync(input.FromStationId);
            var toName = await _stationRepository.GetNameAsync(input.ToStationId);
            return new BlazorJsonResponse
            {
                Status = "1",
                Message = $"take a seat success {seatId} [{fromName} -> {toName}] with requestId : {requestId}"
            };
        }

        /// <summary>
        /// Get current seat count
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<GetSeatOutput> GetSeatAsync([FromQuery] GetSeatInput input)
        {
            var trains = await _trainInfoRepository.GetTrainsAsync(input.FromStationId, input.ToStationId);
            var tasks = trains.Select(async x =>
            {
                var trainId = x;
                var trainGran = _actorProxyFactory.GetClaptrap<ITrainGran>(x.ToString());
                var leftCount = await trainGran
                    .GetLeftSeatCountAsync(input.FromStationId, input.ToStationId);
                var trainInfo = await _trainInfoRepository.GetTrainInfoAsync(trainId);
                var item = new SeatListItem
                {
                    LeftCount = leftCount,
                    TrainId = trainId,
                    FromStationId = trainInfo.FromStationId,
                    ToStationId = trainInfo.ToStationId,
                    FromStationName = await _stationRepository.GetNameAsync(trainInfo.FromStationId),
                    ToStationName = await _stationRepository.GetNameAsync(trainInfo.ToStationId)
                };
                return item;
            });
            var items = await Task.WhenAll(tasks);

            var re = new GetSeatOutput
            {
                Items = items,
                FromStationId = input.FromStationId,
                ToStationId = input.ToStationId,
                FromStationName = await _stationRepository.GetNameAsync(input.FromStationId),
                ToStationName = await _stationRepository.GetNameAsync(input.ToStationId)
            };
            return re;
        }
    }
}