using System.Threading.Tasks;
using Newbe.Claptrap.ArticleManager.IGrains;
using Newbe.Claptrap.Preview.Attributes;
using Newbe.Claptrap.Preview.Orleans;
using Newbe.Claptrap.Preview.StorageProvider.SQLite;

namespace Newbe.Claptrap.ArticleManager.Grains
{
    [ClaptrapStateInitialFactoryHandler]
    [EventStore(typeof(SQLiteEventStoreFactory), typeof(SQLiteEventStoreFactory))]
    [StateStore(typeof(SQLiteStateStoreFactory), typeof(SQLiteStateStoreFactory))]
    [ClaptrapEventHandler(typeof(UpdateArticleEventHandler), typeof(UpdateArticleEventData))]
    public class ArticleGrain : ClaptrapBox<ArticleStateData>, IArticleGrain
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
            await Claptrap.HandleEvent(dataEvent);
            return StateData.Current;
        }
    }
}