using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy.RepositoryPattern;
using ZKEACMS.Search.Models;
using Easy.Extend;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

namespace ZKEACMS.Search.Service
{
    public class MsSqlFullTextSearchService : ISearchService
    {
        public string SearchEngine => "MsSqlFullTextSearch";

        private readonly WebPageDbContext DbContext;
        public MsSqlFullTextSearchService(IOptions<SearchOption> databaseOption)
        {
            DbContext = new WebPageDbContext(databaseOption.Value.ConnectionString);
        }
        private string ToQueryString(string q)
        {
            q = q.Replace("\"", "");
            var keyWords = q.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < keyWords.Length; i++)
            {
                keyWords[i] = "\"{0}\"".FormatWith(keyWords[i]);
            }
            return string.Join(" AND ", keyWords);
        }
        public string[] GetKeyWords(string q)
        {
            List<string> keywords = new List<string>();
            q = ToQueryString(q);
            var dbConnection = DbContext.Database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = "SELECT display_term FROM sys.dm_fts_parser(@Query, 2052, 0, 0);";
                command.Parameters.Add(new SqlParameter("@Query", q));
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    keywords.Add(reader.GetString(0));
                }
            }
            return keywords.ToArray();
        }

        public IList<WebPage> Search(string q, Pagination pagination)
        {
            pagination.PageSize = 10;
            if (q.IsNullOrEmpty())
            {
                return new List<WebPage>();
            }
            q = ToQueryString(q);
            var dbConnection = DbContext.Database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(1) FROM WebPages T0 INNER JOIN CONTAINSTABLE(WebPages,(Title,KeyWords,MetaDescription,PageContent),@Query) T1 ON T0.Url=T1.[KEY]";
                command.Parameters.Add(new SqlParameter("@Query", q));
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }
                var scalar = command.ExecuteScalar();
                pagination.RecordCount = (int)scalar;
            }

            var result = DbContext.WebPage.FromSql(new RawSqlString(
                              @"SELECT T0.Url,T0.Title,T0.KeyWords,T0.MetaDescription,T0.PageContent,T0.Status,T0.Description,T0.CreateBy,T0.CreatebyName,T0.CreateDate,T0.LastUpdateBy,T0.LastUpdateByName,T0.LastUpdateDate FROM 
                            WebPages T0 INNER JOIN CONTAINSTABLE(WebPages,(Title,KeyWords,MetaDescription,PageContent),@Query) T1 on T0.Url=T1.[KEY]
                            ORDER by T1.[RANK] DESC
                            OFFSET @PageSize * @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY;"),
                              new SqlParameter("@Query", q), new SqlParameter("@PageSize", pagination.PageSize), new SqlParameter("@PageIndex", pagination.PageIndex));
            return result.ToList();
        }
    }
}
