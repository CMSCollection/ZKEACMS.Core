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
    public class WebPageService : ServiceBase<WebPage>, IWebPageService
    {
        public WebPageService(IApplicationContext applicationContext, IOptions<SearchOption> databaseOption, WebPageDbContext dbContext)
            : base(applicationContext, dbContext)
        {
            DbContext = new WebPageDbContext(databaseOption.Value.ConnectionString);
        }
        public override DbSet<WebPage> CurrentDbSet => (DbContext as WebPageDbContext).WebPage;
    }
}
