/* http://www.zkea.net/ 
 * Copyright 2017 
 * ZKEASOFT 
 * http://www.zkea.net/licenses 
 */


using Microsoft.EntityFrameworkCore;
using ZKEACMS.Search.Models;

namespace ZKEACMS.Search
{
    public class WebPageDbContext : DbContext
    {
        private string _connectionString;
        public WebPageDbContext()
        {
        }
        public WebPageDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        internal DbSet<WebPage> WebPage { get; set; }
    }
}