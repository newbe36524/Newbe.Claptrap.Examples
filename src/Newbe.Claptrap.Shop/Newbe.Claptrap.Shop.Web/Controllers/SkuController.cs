using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.IGrain.Domain.Sku.Master;
using Newbe.Claptrap.Shop.Models;
using Newbe.Claptrap.Shop.Repository;
using Orleans;

namespace Newbe.Claptrap.Shop.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkuController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;
        private readonly ISkuRepository _skuRepository;

        public SkuController(
            IGrainFactory grainFactory,
            ISkuRepository skuRepository)
        {
            _grainFactory = grainFactory;
            _skuRepository = skuRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ActivateAllAsync()
        {
            var sw = Stopwatch.StartNew();
            await Task.WhenAll(ActivateAllAsync().ToEnumerable());
            sw.Stop();
            return Content($"activated all grains. cost {sw.ElapsedMilliseconds} ms");

            IAsyncEnumerable<Task> ActivateAllAsync()
            {
                return GetAllIdsAsync()
                    .Select(id =>
                    {
                        var skuGrain = _grainFactory.GetGrain<ISkuGrain>(id);
                        return skuGrain.SetupAsync();
                    });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SoldAllAsync(int userCount = 10)
        {
            var rd = new Random();
            var userIds = Enumerable.Range(0, userCount).Select(x => x.ToString()).ToArray();

            var sw = Stopwatch.StartNew();
            var tasks = GetAllIdsAsync()
                .Select(skuId => (skuId, userId: userIds[rd.Next(0, userIds.Length)]))
                .Select(async (tuple, i) =>
                {
                    var (skuId, userId) = tuple;
                    var skuGrain = _grainFactory.GetGrain<ISkuGrain>(skuId);
                    var status = await skuGrain.GetStatusAsync();
                    if (status == SkuStatus.OnSell)
                    {
                        await skuGrain.SoldAsync(userId);
                    }
                })
                .ToEnumerable();

            await Task.WhenAll(tasks);

            sw.Stop();

            return Content($"user count {userCount}. all sku sold, cost {sw.ElapsedMilliseconds} ms");
        }

        private async IAsyncEnumerable<string> GetAllIdsAsync()
        {
            var pageIndex = 1;
            const int pageSize = 1000;
            var getMore = true;
            while (getMore)
            {
                getMore = false;
                var ids = await _skuRepository.GetSkuIdsAsync(pageIndex, pageSize);
                foreach (var id in ids)
                {
                    getMore = true;
                    yield return id;
                }

                pageIndex++;
            }
        }
    }
}