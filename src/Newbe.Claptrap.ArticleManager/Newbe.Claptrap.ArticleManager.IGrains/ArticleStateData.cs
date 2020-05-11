using System.Collections.Generic;

namespace Newbe.Claptrap.ArticleManager.IGrains
{
    public class ArticleStateData : IStateData
    {
        public ArticleData Current { get; set; }
        public IList<ArticleData> ArticleHistories { get; set; }
    }
}