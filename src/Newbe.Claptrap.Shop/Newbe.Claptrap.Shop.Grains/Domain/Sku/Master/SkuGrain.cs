using System;
using System.Threading.Tasks;
using Newbe.Claptrap.IGrain;
using Newbe.Claptrap.IGrain.Domain.Sku.Events;
using Newbe.Claptrap.IGrain.Domain.Sku.Master;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Shop.Models;

namespace Newbe.Claptrap.Shop.Grains.Domain.Sku.Master
{
    [ClaptrapStateInitialFactoryHandler(typeof(SkuStateDataInitFactoryHandler))]
    [ClaptrapEventHandler(typeof(SkuSoldEventHandler), ClaptrapCodes.SkuSoldEvent)]
    public class SkuGrain : ClaptrapBoxGrain<SkuStateData>, ISkuGrain
    {
        private readonly IClock _clock;

        public SkuGrain(IClaptrapGrainCommonService claptrapGrainCommonService
            , IClock clock)
            : base(claptrapGrainCommonService)
        {
            _clock = clock;
        }

        public Task<SoldResult> SoldAsync(string buyerUserId)
        {
            return StateData.Status switch
            {
                SkuStatus.OnSell => Sold(),
                SkuStatus.Sold => Task.FromResult(SoldResult.AlreadySold),
                _ => throw new ArgumentOutOfRangeException()
            };

            async Task<SoldResult> Sold()
            {
                var @event = this.CreateEvent(new SkuSoldEventData
                {
                    BuyerUserId = buyerUserId,
                    SoldTime = _clock.UtcNow,
                });
                await Claptrap.HandleEventAsync(@event);
                return SoldResult.Success;
            }
        }

        public Task SetupAsync()
        {
            return Task.CompletedTask;
        }

        public Task<SkuStatus> GetStatusAsync()
        {
            return Task.FromResult(StateData.Status);
        }
    }
}