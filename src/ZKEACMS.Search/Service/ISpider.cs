using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZKEACMS.Search.Service
{
    public interface ISpider
    {
        Uri StartUri { get; set; }
        string Host { get; set; }
        Task Start(string url);
    }
}
