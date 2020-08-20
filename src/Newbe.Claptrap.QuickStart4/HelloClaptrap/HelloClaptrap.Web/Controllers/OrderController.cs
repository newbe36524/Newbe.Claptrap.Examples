using System;
using System.Threading.Tasks;
using HelloClaptrap.IActor;
using HelloClaptrap.Models.Order;
using HelloClaptrap.Repository;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace HelloClaptrap.Web.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IGrainFactory _grainFactory;
        private readonly IOrderRepository _orderRepository;

        public OrderController(
            IGrainFactory grainFactory,
            IOrderRepository orderRepository)
        {
            _grainFactory = grainFactory;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var re = await _orderRepository.GetAllAsync();
            return Json(re);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrderAsync([FromBody] CreateOrderInput input)
        {
            var orderId = Guid.NewGuid().ToString("N");
            var orderGrain = _grainFactory.GetGrain<IOrderGrain>(orderId);
            await orderGrain.CreateOrderAsync(input);
            return Json("ok");
        }
    }
}