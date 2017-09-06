using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy;
using ZKEACMS.Search.Models;
using ZKEACMS.Widget;

namespace ZKEACMS.Search.Service
{
    public class SearchWidgetService : SimpleWidgetService<SearchWidget>
    {
        public SearchWidgetService(IWidgetBasePartService widgetBasePartService, IApplicationContext applicationContext) : 
            base(widgetBasePartService, applicationContext)
        {
        }
    }
}
