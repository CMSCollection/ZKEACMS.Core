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
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace ZKEACMS.Search.Controllers
{

    public class SearchController : Controller
    {
        private readonly IWebPageService _webPageService;
        private readonly IOptions<SearchOption> _searchOption;
        public SearchController(IWebPageService service, IOptions<SearchOption> searchOption)
        {
            _webPageService = service;
            _searchOption = searchOption;
        }
        public IActionResult StartIndex()
        {
            IndexProcess.Start(_searchOption.Value.DotNet);
            return Json(true);
        }
        public IActionResult Status()
        {
            
            return Json(IndexProcess.OutputMessage);
        }
    }
}
