/* http://www.zkea.net/ 
 * Copyright 2017 ZKEASOFT 
 * http://www.zkea.net/licenses 
 *
 */
using Easy.Mvc.Resource;
using Easy.Mvc.Route;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using ZKEACMS.Search.Service;

namespace ZKEACMS.Search
{
    public class SearchPlug : PluginBase
    {
        public override IEnumerable<RouteDescriptor> RegistRoute()
        {
            yield return new RouteDescriptor
            {
                RouteName = "SiteSearch",
                Template = "search",
                Defaults = new { controller = "SiteSearch", action = "Search", module = "Search" },
                Priority = 11
            };
        }

        public override IEnumerable<AdminMenu> AdminMenu()
        {
            yield return new AdminMenu
            {
                Title = "È«Õ¾ËÑË÷",
                Url = "~/Admin/SiteSearch",
                PermissionKey = PermissionKeys.ManageSearch,
                Icon = "glyphicon-search",
                Order = 13
            };
        }

        protected override void InitScript(Func<string, ResourceHelper> script)
        {

        }

        protected override void InitStyle(Func<string, ResourceHelper> style)
        {
            style("Search").Include("~/Plugins/ZKEACMS.Search/Content/search.css", "~/Plugins/ZKEACMS.Search/Content/search.min.css");
        }

        public override IEnumerable<PermissionDescriptor> RegistPermission()
        {
            yield return new PermissionDescriptor
            {
                Title = "¸üÐÂÍøÕ¾Ë÷Òý",
                Module = "È«Õ¾ËÑË÷",
                Key = PermissionKeys.ManageSearch
            };
        }

        public override IEnumerable<Type> WidgetServiceTypes()
        {
            yield return typeof(SearchWidgetService);
        }

        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<ISpider, Spider>();
            serviceCollection.TryAddTransient<IWebPageService, WebPageService>();
            serviceCollection.AddTransient<ISearchService, MsSqlFullTextSearchService>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(CurrentPluginPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables().Build();

            serviceCollection.Configure<SearchOption>(configuration);
        }
    }
}