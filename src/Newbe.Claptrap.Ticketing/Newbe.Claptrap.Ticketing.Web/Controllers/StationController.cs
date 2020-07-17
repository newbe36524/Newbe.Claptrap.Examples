using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Ticketing.Repository;
using Newbe.Claptrap.Ticketing.Web.Models;
using Newbe.Claptrap.Ticketing.Web.Models.Api;

namespace Newbe.Claptrap.Ticketing.Web.Controllers
{
    [Route("api/[controller]")]
    public class StationController : Controller
    {
        private readonly IStationRepository _stationRepository;

        public StationController(
            IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        [HttpGet("GetAllStation")]
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

       
    }
}