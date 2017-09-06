/* http://www.zkea.net/ 
 * Copyright 2017 
 * ZKEASOFT 
 * http://www.zkea.net/licenses 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZKEACMS.Search.Models
{
    public class SearchResult
    {
        public string Query { get; set; }
        public IEnumerable<WebPage> WebPages { get; set; }
        public Pagin Pagination { get; set; }
    }
}
