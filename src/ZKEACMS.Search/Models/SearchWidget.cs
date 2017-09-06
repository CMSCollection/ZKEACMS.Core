using Easy.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKEACMS.MetaData;
using ZKEACMS.Widget;

namespace ZKEACMS.Search.Models
{
    [ViewConfigure(typeof(SearchWidgetMetaData))]
    public class SearchWidget : SimpleWidgetBase
    {
    }
    class SearchWidgetMetaData : WidgetMetaData<SearchWidget>
    {

    }
}
