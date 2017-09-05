using Easy.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKEACMS.Search.Models;

namespace ZKEACMS.Search.Service
{
    public interface IWebPageService : IService<WebPage>
    {
        IEnumerable<WebPage> Search(string q, Pagination pagination);
    }
}
