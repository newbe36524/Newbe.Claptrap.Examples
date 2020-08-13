using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Ticketing.Repository;
using Newbe.Claptrap.Ticketing.Web.Models;

namespace Newbe.Claptrap.Ticketing.Web.Controllers
{
    /// <summary>
    /// Station Api
    /// </summary>
    [Route("api/[controller]")]
    public class StationController : Controller
    {
        private readonly IStationRepository _stationRepository;

        public StationController(
            IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        /// <summary>
        /// Get All Station Info
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllStation")]
        public async Task<PassStation[]> GetAllStationAsync()
        {
            var nameDic = await _stationRepository.GetAllNameAsync();
            var re = nameDic.Select(x => new PassStation
                {
                    StationId = x.Key,
                    StationName = x.Value
                })
                .ToArray();
            return re;
        }
    }
}