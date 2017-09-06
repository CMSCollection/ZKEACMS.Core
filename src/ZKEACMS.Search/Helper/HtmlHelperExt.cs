using Easy.Extend;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ZKEACMS.Search
{
    public static class HtmlHelperExt
    {
        public static IHtmlContent StrongKeyWord(this IHtmlHelper htmlHelper, string content, string keyWorld)
        {
            var keyWorlds = keyWorld.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in keyWorlds)
            {
                var index = content.IndexOf(item, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    content = content.Replace(item, "<strong>{0}</strong>".FormatWith(content.Substring(index,item.Length)), StringComparison.OrdinalIgnoreCase);
                }
                
            }
            return new HtmlString(content);
        }
        public static IHtmlContent ExtractDescription(this IHtmlHelper htmlHelper, string content, string keyWorld)
        {
            var keyWorlds = keyWorld.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            int index = 0;
            foreach (var item in keyWorld)
            {
                index = content.IndexOf(item);
                if (index >= 0)
                {
                    break;
                }
            }
            if (index < 0)
            {
                index = 0;
            }
            if (content.Length >= index + 150)
            {
                return htmlHelper.StrongKeyWord(content.Substring(index, 150), keyWorld);
            }
            else
            {
                return htmlHelper.StrongKeyWord(content.Substring(index, content.Length - index), keyWorld);
            }
        }
    }
}
