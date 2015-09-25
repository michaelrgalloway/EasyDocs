using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyDocs.ApiModels.ReadOnly
{
    public class PageView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string SideBarContent { get; set; }
        public int Order { get; set; }
        public string UrlKey { get; set; }
        public string Draft { get; set; }
        public bool Active { get; set; }
    }
}