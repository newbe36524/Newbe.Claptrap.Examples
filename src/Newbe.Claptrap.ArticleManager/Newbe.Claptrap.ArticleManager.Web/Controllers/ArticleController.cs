using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.ArticleManager.IGrains;
using Orleans;

namespace Newbe.Claptrap.ArticleManager.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IGrainFactory _grainFactory;

        public ArticleController(
            IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> Get(string articleId)
        {
            var articleGrain = _grainFactory.GetGrain<IArticleGrain>(articleId);
            var articleData = await articleGrain.GetCurrentAsync();
            if (articleData == null)
            {
                return Content($"article {articleId} is not created, please create it at first");
            }

            return Content(FormatArticle(articleData));
        }

        [HttpPost("article/{articleId}")]
        public async Task<IActionResult> Update(string articleId, [FromBody] ArticleData articleData)
        {
            var articleGrain = _grainFactory.GetGrain<IArticleGrain>(articleId);
            var result = await articleGrain.UpdateAsync(articleData);
            return Content(
                @$"Article Updated! 
{FormatArticle(result)}");
        }

        private static string FormatArticle(ArticleData articleData)
            =>
                @$"《{articleData.Title}》 update at : {articleData.CreateTime}
{articleData.Content}";
    }
}