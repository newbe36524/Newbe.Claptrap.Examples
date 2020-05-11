using System.Threading.Tasks;
using Newbe.Claptrap.ArticleManager.IGrains;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.ArticleManager.Grains
{
    [ClaptrapStateInitialFactoryHandler]
    [ClaptrapEventHandler(typeof(UpdateArticleEventHandler), ClaptrapCodes.UpdateArticleEvent)]
    public class ArticleGrain : ClaptrapBoxGrain<ArticleStateData>, IArticleGrain
    {
        public ArticleGrain(IClaptrapGrainCommonService claptrapGrainCommonService)
            : base(claptrapGrainCommonService)
        {
        }

        public Task<ArticleData> GetCurrentAsync()
        {
            return Task.FromResult(StateData.Current);
        }

        public async Task<ArticleData> UpdateAsync(ArticleData articleData)
        {
            var dataEvent = this.CreateEvent(new UpdateArticleEventData
            {
                ArticleData = articleData
            });
            await Claptrap.HandleEventAsync(dataEvent);
            return StateData.Current;
        }
    }
}