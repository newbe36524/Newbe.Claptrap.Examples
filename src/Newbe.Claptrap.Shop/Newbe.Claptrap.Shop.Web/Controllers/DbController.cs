using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Shop.Repository;

namespace Newbe.Claptrap.Shop.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DbController : ControllerBase
    {
        private readonly IDbManager _dbManager;

        public DbController(
            IDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        [HttpGet]
        public async Task<IActionResult> InitAsync()
        {
            await _dbManager.InitAsync();
            return Content("db created success");
        }
    }
}