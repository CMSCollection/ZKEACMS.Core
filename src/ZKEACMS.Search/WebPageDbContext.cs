using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ZKEACMS.Search.Models;

namespace ZKEACMS.Search
{
    public class WebPageDbContext : CMSDbContext
    {
        internal DbSet<WebPage> WebPage { get; set; }
    }
}