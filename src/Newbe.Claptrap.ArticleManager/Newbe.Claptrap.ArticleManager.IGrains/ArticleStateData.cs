using System.Collections.Generic;
using Newbe.Claptrap.Preview.Abstractions.Core;

namespace Newbe.Claptrap.ArticleManager.IGrains
{
    public class ArticleStateData : IStateData
    {
        public ArticleData Current { get; set; }
        public IList<ArticleData> ArticleHistories { get; set; }
    }
}