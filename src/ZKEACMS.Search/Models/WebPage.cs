/* http://www.zkea.net/ 
 * Copyright 2017 
 * ZKEASOFT 
 * http://www.zkea.net/licenses 
 */


using Easy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZKEACMS.Search.Models
{
    [Table("WebPages")]
    public class WebPage : EditorEntity
    {
        [Key]
        public string Url { get; set; }
        public string KeyWords { get; set; }
        public string MetaDescription { get; set; }
        public string PageContent { get; set; }
    }
}