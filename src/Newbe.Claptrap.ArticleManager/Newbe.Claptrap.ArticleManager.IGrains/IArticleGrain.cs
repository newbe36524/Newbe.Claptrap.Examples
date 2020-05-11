using System.Threading.Tasks;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.ArticleManager.IGrains
{
    [ClaptrapState(typeof(ArticleStateData), ClaptrapCodes.Account)]
    [ClaptrapEvent(typeof(UpdateArticleEventData), ClaptrapCodes.UpdateArticleEvent)]
    public interface IArticleGrain : IClaptrapGrain
    {
        Task<ArticleData> GetCurrentAsync();
        Task<ArticleData> UpdateAsync(ArticleData articleData);
    }
}