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
    /// <summary>
    /// Train Api
    /// </summary>
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

        /// <summary>
        /// Get Left Seat Count of a train
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        [HttpGet("GetLeftSeat")]
        public async Task<IEnumerable<LeftCountItem>> GetLeftSeatAsync(int trainId)
        {
            var trainGran = _grainFactory.GetGrain<ITrainGran>(trainId.ToString());
            var allCount = await trainGran.GetAllCountAsync();
            var stationIds = allCount.Keys
                .Concat(allCount.SelectMany(x => x.Value.Keys))
                .Distinct();
            var nameDic = await _stationRepository.GetNamesAsync(stationIds);

            var re = allCount.SelectMany(x =>
                {
                    var (key, value) = x;
                    return value.Select(inner => new LeftCountItem
                    {
                        LeftCount = inner.Value,
                        FromStationId = key,
                        FromStationName = nameDic[key],
                        ToStationId = inner.Key,
                        ToStationName = nameDic[inner.Key]
                    });
                })
                .ToList();
            return re;
        }

        /// <summary>
        /// Get All Trains Info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TrainBasicInfoViewModel[]> GetAllTrainInfoAsync()
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
            return re;
        }
    }
}