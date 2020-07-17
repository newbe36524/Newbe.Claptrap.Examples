using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Repository;
using Newbe.Claptrap.Ticketing.Web.Models;
using Newbe.Claptrap.Ticketing.Web.Models.Api;
using Orleans;

namespace Newbe.Claptrap.Ticketing.Web.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("GetLeftSeat")]
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
    }
}