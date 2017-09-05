using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy;
using Easy.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using ZKEACMS.Search.Models;
using Microsoft.Extensions.Options;

namespace ZKEACMS.Search.Service
{
    public class WebPageService : ServiceBase<WebPage, WebPageDbContext>, IWebPageService
    {
        public WebPageService(IApplicationContext applicationContext, IOptions<SearchOption> databaseOption) : base(applicationContext)
        {
            DbContext = new WebPageDbContext(databaseOption.Value.ConnectionString);
        }
        public override WebPageDbContext DbContext { get; set; }
        public override DbSet<WebPage> CurrentDbSet => DbContext.WebPage;

        public IEnumerable<WebPage> Search(string q, Pagination pagination)
        {
            return CurrentDbSet.Where(m => EF.Functions.Like(m.Title, q)).ToList();
        }
    }
}
