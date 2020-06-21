using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Auth.IGrains;
using Orleans;

namespace Newbe.Claptrap.Auth.WebApi.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IGrainFactory _grainFactory;

        public UserController(
            IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Login(string id, string username, string password)
        {
            var userGrain = _grainFactory.GetGrain<IUserGrain>(id);
            var token = await userGrain.LoginAsync(username, password);
            return Json(new
            {
                token,
                validate = $"/api/user/{id}/validate?token={WebUtility.UrlEncode(token)}"
            });
        }

        [HttpGet("{id}/validate")]
        public async Task<IActionResult> Validate(string id, string token)
        {
            var userGrain = _grainFactory.GetGrain<IUserGrain>(id);
            var validateResult = await userGrain.ValidateAsync(token);
            return Json(validateResult);
        }
    }
}