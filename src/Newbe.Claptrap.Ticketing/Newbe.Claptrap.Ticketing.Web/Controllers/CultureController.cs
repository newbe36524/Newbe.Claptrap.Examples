using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Newbe.Claptrap.Ticketing.Web.Controllers
{
    /// <summary>
    /// Culture Api to change current culture
    /// </summary>
    [Route("[controller]/[action]")]
    public class CultureController : Controller
    {
        /// <summary>
        /// Set current UI culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SetCulture(string culture, string redirectUri)
        {
            if (culture != null)
            {
                HttpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(
                        new RequestCulture(culture)));
            }

            return LocalRedirect(redirectUri);
        }
    }
}