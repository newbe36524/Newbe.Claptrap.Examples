using System;
using System.Threading.Tasks;
using Newbe.Claptrap.Preview.Attributes;
using Newbe.Claptrap.Preview.Orleans;

namespace Newbe.Claptrap.ArticleManager.IGrains
{
    [ClaptrapState(typeof(ArticleStateData))]
    [ClaptrapEvent(typeof(UpdateArticleEventData))]
    public interface IArticleGrain : IClaptrapGrain
    {
        Task<ArticleData> GetCurrentAsync();
        Task<ArticleData> UpdateAsync(ArticleData articleData);
    }
}