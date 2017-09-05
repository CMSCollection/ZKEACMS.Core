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
        public WebPageService(IApplicationContext applicationContext, IOptions<DatabaseOption> databaseOption) : base(applicationContext)
        {
            DbContext = new WebPageDbContext(databaseOption.Value.DefaultConnection);
        }
        public override WebPageDbContext DbContext { get; set; }
        public override DbSet<WebPage> CurrentDbSet => DbContext.WebPage;
    }
}
