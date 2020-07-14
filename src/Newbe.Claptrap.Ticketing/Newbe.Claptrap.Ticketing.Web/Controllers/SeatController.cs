using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Repository;
using Orleans;

namespace Newbe.Claptrap.Ticketing.Web.Controllers
{
    [Route("api/[controller]")]
    public class SeatController : Controller
    {
        private readonly IGrainFactory _grainFactory;
        private readonly IStationRepository _stationRepository;
        private readonly ITrainInfoRepository _trainInfoRepository;

        public SeatController(
            IGrainFactory grainFactory,
            IStationRepository stationRepository,
            ITrainInfoRepository trainInfoRepository)
        {
            _grainFactory = grainFactory;
            _stationRepository = stationRepository;
            _trainInfoRepository = trainInfoRepository;
        }

        [HttpPost("{seatId}")]
        public async Task<IActionResult> TakeSeatAsync(int seatId, [FromBody] TakeSeatInput input)
        {
            var cartGrain = _grainFactory.GetGrain<ISeatGrain>(seatId.ToString());
            var requestId = Guid.NewGuid().ToString("N");
            try
            {
                await cartGrain.TakeSeatAsync(input.FromStationId, input.ToStationId, requestId);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            var fromName = await _stationRepository.GetNameAsync(input.FromStationId);
            var toName = await _stationRepository.GetNameAsync(input.ToStationId);
            return Json($"take a seat success {seatId} [{fromName} -> {toName}] with requestId : {requestId}");
        }

        [HttpGet]
        public async Task<IActionResult> GetSeatAsync([FromQuery] GetSeatInput input)
        {
            var trains = await _trainInfoRepository.GetTrainsAsync(input.FromStationId, input.ToStationId);
            var tasks = trains.Select(async x =>
            {
                var trainId = x;
                var trainGran = _grainFactory.GetGrain<ITrainGran>(x.ToString());
                var leftCount = await trainGran
                    .GetLeftSeatCountAsync(input.FromStationId, input.ToStationId);
                return (trainId, leftCount);
            });
            var taskResult = await Task.WhenAll(tasks);
            var items = taskResult.Select(x => new SeatListItem
            {
                TrainId = x.trainId,
                LeftCount = x.leftCount
            });
            var re = new GetSeatOutput
            {
                Items = items,
                FromStationId = input.FromStationId,
                ToStationId = input.ToStationId,
                FromStationName = await _stationRepository.GetNameAsync(input.FromStationId),
                ToStationName = await _stationRepository.GetNameAsync(input.ToStationId)
            };
            return Json(re);
        }

        public class GetSeatInput
        {
            public int FromStationId { get; set; }
            public int ToStationId { get; set; }
        }

        public class GetSeatOutput
        {
            public int FromStationId { get; set; }
            public int ToStationId { get; set; }
            public string FromStationName { get; set; }
            public string ToStationName { get; set; }
            public IEnumerable<SeatListItem> Items { get; set; }
        }

        public class SeatListItem
        {
            public int TrainId { get; set; }
            public int LeftCount { get; set; }
        }

        public class TakeSeatInput
        {
            public int FromStationId { get; set; }
            public int ToStationId { get; set; }
        }
    }
}