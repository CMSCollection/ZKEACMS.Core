using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ZKEACMS.Search.Models;
using Easy.Extend;

namespace ZKEACMS.Search.Service
{
    public class Spider : ISpider
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3198.0 Safari/537.36";
        private readonly IWebPageService _webPageService;
        public Spider(IWebPageService webPageService)
        {
            _webPageService = webPageService;
            loads = new HashSet<string>();
        }
        public Uri StartUri { get; set; }
        public string Host { get; set; }
        private HashSet<string> loads;
        public Task Start(string url)
        {
            StartUri = new Uri(url);
            Host = StartUri.Scheme + "://" + StartUri.Authority;
            return Task.Factory.StartNew(() =>
             {
                 Load(StartUri.ToString());
             });
        }
        private void Load(string url)
        {
            if (string.IsNullOrEmpty(url) || url == "#" || url.StartsWith("javascript:"))
            {
                return;
            }
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = Host + url;
            }
            if (!url.StartsWith(StartUri.ToString()) || loads.Contains(url) || new Uri(url).IsFile)
            {
                return;
            }
			url = WebUtility.UrlDecode(url);
            Console.WriteLine(url);
            var request = WebRequest.Create(url);
            request.Headers["User-Agent"] = UserAgent;

            HttpWebResponse response = null;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                response = e.Response as HttpWebResponse;
            }
            finally
            {
                loads.Add(url);
            }
            string html = string.Empty;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _webPageService.Remove(url);
                }
            }
            if (response.StatusCode == HttpStatusCode.OK && response.ContentType.IndexOf("text/html") >= 0)
            {
                using (var stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                }
            }
            response.Dispose();
            if (html.IsNullOrEmpty())
            {
                return;
            }

            HtmlDocument doc = null;
            try
            {
                doc = new HtmlDocument();
                doc.LoadHtml(html);
                if (!doc.DocumentNode.HasChildNodes)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }


            var title = WebUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode("/html/head/title").InnerText);
            string keywords = string.Empty;
            string description = string.Empty;
            string pageContent = WebUtility.HtmlDecode(doc.DocumentNode.InnerText).NoHTML();
            foreach (var meta in doc.DocumentNode.SelectNodes("/html/head/meta"))
            {
                string metaName = meta.GetAttributeValue("name", "");

                if (metaName.Equals("keywords", StringComparison.OrdinalIgnoreCase))
                {
                    keywords = WebUtility.HtmlDecode(meta.GetAttributeValue("content", ""));
                }
                else if (metaName.Equals("description", StringComparison.OrdinalIgnoreCase))
                {
                    description = WebUtility.HtmlDecode(meta.GetAttributeValue("content", ""));
                }
            }

            var webPage = _webPageService.Get(url);
            if (webPage == null)
            {
                _webPageService.Add(new WebPage { Url = url, Title = title, KeyWords = keywords, MetaDescription = description, PageContent = pageContent });
            }
            else
            {
                webPage.Title = title;
                webPage.KeyWords = keywords;
                webPage.MetaDescription = description;
                webPage.PageContent = pageContent;
                _webPageService.Update(webPage);
            }
            var links = doc.DocumentNode.SelectNodes("//a");
            foreach (var item in links)
            {
                var href = item.GetAttributeValue("href", "");
                Load(HtmlEntity.DeEntitize(href));
            }

        }
    }
}
