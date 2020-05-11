using System;
using System.Threading.Tasks;
using Newbe.Claptrap.IGrain;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.Shop.Grains
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
                var @event = this.CreateEvent(new SkuSoldEvent
                {
                    BuyerUserId = buyerUserId,
                    SoldTime = _clock.UtcNow,
                });
                await Claptrap.HandleEventAsync(@event);
                return SoldResult.Success;
            }
        }
    }
}