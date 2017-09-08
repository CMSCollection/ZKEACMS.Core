using Easy.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKEACMS.Search.Models;

namespace ZKEACMS.Search.Service
{
    public interface ISearchService
    {
        string SearchEngine { get; }
        string[] GetKeyWords(string q);
        IList<WebPage> Search(string q, Pagination pagination);
    }
}
