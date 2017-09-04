/*!
 * http://www.zkea.net/
 * Copyright 2017 ZKEASOFT
 * http://www.zkea.net/licenses
 */

using Easy.Constant;
using Easy.Mvc.Authorize;
using Easy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using ZKEACMS.Article.Models;
using ZKEACMS.Article.Service;
using Easy.Extend;
using ZKEACMS.Search.Models;
using ZKEACMS.Search.Service;
using System.Threading.Tasks;

namespace ZKEACMS.Search.Controllers
{

    public class SearchController : BasicController<WebPage, string, IWebPageService>
    {
        private readonly ISpider _spider;
        public SearchController(IWebPageService service, ISpider spider) : base(service)
        {
            _spider = spider;
        }

        public async Task<IActionResult> Test()
        {
            await _spider.Start("http://www.kingstong.com/cn");
            return View();
        }
    }
}
