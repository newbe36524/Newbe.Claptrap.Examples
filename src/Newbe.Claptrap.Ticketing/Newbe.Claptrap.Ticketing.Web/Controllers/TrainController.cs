using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Repository;
using Newbe.Claptrap.Ticketing.Web.Models;
using Orleans;

namespace Newbe.Claptrap.Ticketing.Web.Controllers
{
    [Route("api/[controller]")]
    public class TrainController : Controller
    {
        private readonly IGrainFactory _grainFactory;
        private readonly ITrainInfoRepository _trainInfoRepository;
        private readonly IStationRepository _stationRepository;

        public TrainController(
            IGrainFactory grainFactory,
            ITrainInfoRepository trainInfoRepository,
            IStationRepository stationRepository)
        {
            _grainFactory = grainFactory;
            _trainInfoRepository = trainInfoRepository;
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
                .ToList();
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

        [HttpGet]
        public async Task<IActionResult> GetAllTrainInfoAsync()
        {
            var allTrainInfo = await _trainInfoRepository.GetAllTrainInfoAsync();
            var tasks = allTrainInfo
                .Select(async x => new TrainBasicInfoViewModel
                {
                    TrainId = x.TrainId,
                    FromStationId = x.FromStationId,
                    FromStationName = await _stationRepository.GetNameAsync(x.FromStationId),
                    ToStationId = x.ToStationId,
                    ToStationName = await _stationRepository.GetNameAsync(x.ToStationId)
                });
            var re = await Task.WhenAll(tasks);
            return Json(re);
        }
    }
}