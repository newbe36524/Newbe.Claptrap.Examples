using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Formatters.Ascii;
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
        private readonly ILocationRepository _locationRepository;
        private readonly ITrainInfoRepository _trainInfoRepository;

        public SeatController(
            IGrainFactory grainFactory,
            ILocationRepository locationRepository,
            ITrainInfoRepository trainInfoRepository)
        {
            _grainFactory = grainFactory;
            _locationRepository = locationRepository;
            _trainInfoRepository = trainInfoRepository;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddItemAsync(int id, [FromBody] TakeSeatInput input)
        {
            var cartGrain = _grainFactory.GetGrain<ISeatGrain>(id.ToString());
            var requestId = Guid.NewGuid().ToString("N");
            try
            {
                await cartGrain.TakeSeatAsync(input.FromLocationId, input.ToLocationId, requestId);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            var fromName = await _locationRepository.GetNameAsync(input.FromLocationId);
            var toName = await _locationRepository.GetNameAsync(input.ToLocationId);
            return Json($"take a seat success {id} [{fromName} -> {toName}] with requestId : {requestId}");
        }

        [HttpGet]
        public async Task<IActionResult> GetSeatAsync([FromBody] GetSeatInput input)
        {
            var trains = await _trainInfoRepository.GetTrainsAsync(input.FromLocationId, input.ToLocationId);
            var tasks = trains.Select(async x =>
            {
                var trainId = x;
                var trainGran = _grainFactory.GetGrain<ITrainGran>(x.ToString());
                var leftCount = await trainGran
                    .GetLeftSeatCountAsync(input.FromLocationId, input.ToLocationId);
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
                FromLocationId = input.FromLocationId,
                ToLocationId = input.ToLocationId,
                FromLocationName = await _locationRepository.GetNameAsync(input.FromLocationId),
                ToLocationName = await _locationRepository.GetNameAsync(input.ToLocationId)
            };
            return Json(re);
        }

        public class GetSeatInput
        {
            public int FromLocationId { get; set; }
            public int ToLocationId { get; set; }
        }

        public class GetSeatOutput
        {
            public int FromLocationId { get; set; }
            public int ToLocationId { get; set; }
            public string FromLocationName { get; set; }
            public string ToLocationName { get; set; }
            public IEnumerable<SeatListItem> Items { get; set; }
        }

        public class SeatListItem
        {
            public int TrainId { get; set; }
            public int LeftCount { get; set; }
        }

        public class TakeSeatInput
        {
            public int FromLocationId { get; set; }
            public int ToLocationId { get; set; }
        }
    }
}