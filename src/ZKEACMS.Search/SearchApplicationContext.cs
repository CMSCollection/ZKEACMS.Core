/* http://www.zkea.net/ 
 * Copyright 2017 
 * ZKEASOFT 
 * http://www.zkea.net/licenses 
 */


using Easy;
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
