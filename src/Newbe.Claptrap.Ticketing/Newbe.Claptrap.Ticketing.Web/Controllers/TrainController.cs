using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Repository;
using Orleans;

namespace Newbe.Claptrap.Ticketing.Web.Controllers
{
    
    [EnableCors("_AllowTrainOrigin")]
    public class TrainController : Controller
    {
        private readonly IGrainFactory _grainFactory;
        private readonly IStationRepository _stationRepository;

        public TrainController(
            IGrainFactory grainFactory,
            IStationRepository stationRepository)
        {
            _grainFactory = grainFactory;
            _stationRepository = stationRepository;
        }
        [Route("api/[controller]/GetLeftSeat")]
        [HttpGet]
        public async Task<IActionResult> GetLeftSeatAsync(int trainId)
        {
            var trainGran = _grainFactory.GetGrain<ITrainGran>(trainId.ToString());
            var allCount = await trainGran.GetAllCountAsync();
            var stationIds = GetStationIds().Distinct();
            var nameDic = await _stationRepository.GetNamesAsync(stationIds);

            var re = allCount.Select(x => new LeftCountItem
                {
                    LeftCount = x.Value,
                    FromStationId = x.Key.FromStationId,
                    FromStationName = nameDic[x.Key.FromStationId],
                    ToStationId = x.Key.ToStationId,
                    ToStationName = nameDic[x.Key.ToStationId]
                })
                .ToArray();
            return Json(re);

            IEnumerable<int> GetStationIds()
            {
                var allCountKeys = allCount.Keys;
                foreach (var allCountKey in allCountKeys)
                {
                    yield return allCountKey.FromStationId;
                    yield return allCountKey.ToStationId;
                }
            }
        }
        [Route("api/[controller]/GetAllStation")]
        [HttpGet]
        public async Task<IActionResult> GetAllStationAsync()
        {
            var nameDic = await _stationRepository.GetAllNameAsync();
            var re = nameDic.Select(x => new PassStation
            {
                StationId = x.Key,
                StationName = x.Value
            })
              .ToArray();
            return Json(re);
        }
        public class PassStation
        {
            public int StationId { get; set; }
            public string StationName { get; set; }
        }
        public class LeftCountItem
        {
            public int FromStationId { get; set; }
            public string FromStationName { get; set; }
            public int ToStationId { get; set; }
            public string ToStationName { get; set; }
            public int LeftCount { get; set; }
        }
    }
}