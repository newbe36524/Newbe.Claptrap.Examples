using System.Threading.Tasks;
using HelloClaptrap.IActor;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace HelloClaptrap.Web.Controllers
{
    [Route("api/[controller]")]
    public class SkuController : Controller
    {
        private readonly IGrainFactory _grainFactory;

        public SkuController(
            IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemsAsync(string id)
        {
            var skuGrain = _grainFactory.GetGrain<ISkuGrain>(id);
            var inventory = await skuGrain.GetInventoryAsync();
            return Json(new
            {
                skuId = id,
                inventory = inventory,
            });
        }
    }
}