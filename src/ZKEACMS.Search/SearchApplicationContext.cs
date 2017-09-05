using Easy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy.Models;
using Microsoft.AspNetCore.Hosting;

namespace ZKEACMS.Search
{
    public class SearchApplicationContext : IApplicationContext
    {
        public IUser CurrentUser { get; set; }

        public IUser CurrentCustomer { get; set; }

        public IHostingEnvironment HostingEnvironment { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
