/*!
 * http://www.zkea.net/
 * Copyright 2017 ZKEASOFT
 * http://www.zkea.net/licenses
 */

using Easy;
using Easy.RepositoryPattern;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZKEACMS.Search.Service;

namespace ZKEACMS.Search
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ISpider, Spider>();
            serviceCollection.AddTransient<IWebPageService, WebPageService>();
            serviceCollection.AddSingleton<IApplicationContext, SearchApplicationContext>();

            var builder = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            var configuration = builder.Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<SearchOption>(configuration);


            var services = serviceCollection.BuildServiceProvider();
            var spider = ActivatorUtilities.GetServiceOrCreateInstance<ISpider>(services);
            var option = ActivatorUtilities.GetServiceOrCreateInstance<IOptions<SearchOption>>(services);
            var task = spider.Start((args == null || args.Length == 0) ? option.Value.Host : args[0]);
            task.Wait();
        }
    }
}
