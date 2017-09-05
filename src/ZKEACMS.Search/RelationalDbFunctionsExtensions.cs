using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZKEACMS.Search
{
    public static class RelationalDbFunctionsExtensions
    {
        public static bool Contains(this DbFunctions functions, string property, string query)
        {
            return true;
        }
    }
}
