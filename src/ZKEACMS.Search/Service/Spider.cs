/* http://www.zkea.net/ 
 * Copyright 2017 
 * ZKEASOFT 
 * http://www.zkea.net/licenses 
 */


using Easy.Extend;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ZKEACMS.Search.Models;

namespace ZKEACMS.Search.Service
{
    public class Spider : ISpider
    {
        class PageResult
        {
            public string RequestUrl { get; set; }
            public string ResponseUrl { get; set; }
            public HtmlDocument Document { get; set; }
            public HttpStatusCode StatusCode { get; set; }
        }
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
                 _webPageService.Get().Each(page => OnePage(page.Url).Wait());
             });
        }
        private void Load(string url)
        {
            var pageResult = LoadPage(url);
            if (pageResult != null)
            {
                Store(pageResult);
                var links = pageResult.Document.DocumentNode.SelectNodes("//a");
                foreach (var item in links)
                {
                    var href = item.GetAttributeValue("href", "");
                    Load(HtmlEntity.DeEntitize(href));
                }
            }

        }

        private PageResult LoadPage(string url)
        {
            if (string.IsNullOrEmpty(url) || url == "#" || url.StartsWith("javascript:"))
            {
                return null;
            }
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = Host + url;
            }
            if (!url.StartsWith(StartUri.ToString()) || loads.Contains(url) || new Uri(url).IsFile)
            {
                return null;
            }
            url = WebUtility.UrlDecode(url);
            Console.WriteLine(url);
            PageResult pageResult = new PageResult();
            pageResult.RequestUrl = url;
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
                if (response == null)
                {
                    return null;
                }
            }
            finally
            {
                loads.Add(url);
                if (response != null)
                {
                    pageResult.ResponseUrl = response.ResponseUri.ToString();
                    pageResult.StatusCode = response.StatusCode;
                    if (pageResult.RequestUrl != pageResult.ResponseUrl && !loads.Contains(pageResult.ResponseUrl))
                    {
                        loads.Add(pageResult.ResponseUrl);
                    }
                }
            }
            string html = string.Empty;
            if (response.ContentType.IndexOf("text/html") >= 0)
            {
                using (response)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            html = reader.ReadToEnd();
                        }
                    }
                }
            }
            if (html.IsNotNullAndWhiteSpace())
            {
                try
                {
                    pageResult.Document = new HtmlDocument();
                    pageResult.Document.LoadHtml(html);
                    if (!pageResult.Document.DocumentNode.HasChildNodes)
                    {
                        pageResult.Document = null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    pageResult.Document = null;
                }
            }
            return pageResult;

        }

        private void Store(PageResult pageResult)
        {
            if (pageResult == null)
            {
                return;
            }
            if (pageResult.StatusCode == HttpStatusCode.NotFound || pageResult.StatusCode == HttpStatusCode.MovedPermanently)
            {
                _webPageService.Remove(pageResult.RequestUrl);
            }
            if (pageResult.Document != null && pageResult.StatusCode != HttpStatusCode.NotFound)
            {
                var title = WebUtility.HtmlDecode(pageResult.Document.DocumentNode.SelectSingleNode("/html/head/title").InnerText);
                string keywords = string.Empty;
                string description = string.Empty;
                string pageContent = WebUtility.HtmlDecode(pageResult.Document.DocumentNode.InnerText).NoHTML();
                foreach (var meta in pageResult.Document.DocumentNode.SelectNodes("/html/head/meta"))
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

                var old = _webPageService.Get(pageResult.ResponseUrl);
                if (old == null)
                {
                    _webPageService.Add(new WebPage { Url = pageResult.ResponseUrl, Title = title, KeyWords = keywords, MetaDescription = description, PageContent = pageContent });
                }
                else
                {
                    old.Title = title;
                    old.KeyWords = keywords;
                    old.MetaDescription = description;
                    old.PageContent = pageContent;
                    _webPageService.Update(old);
                }
            }
        }

        public Task OnePage(string url)
        {
            return Task.Factory.StartNew(() => { Store(LoadPage(url)); });
        }
    }
}
