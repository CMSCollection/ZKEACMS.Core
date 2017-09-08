/* http://www.zkea.net/ 
 * Copyright 2017 
 * ZKEASOFT 
 * http://www.zkea.net/licenses 
 */


using Easy;
using Easy.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ZKEACMS.Search.Models;
using Easy.Extend;
using System;

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
    }
}
