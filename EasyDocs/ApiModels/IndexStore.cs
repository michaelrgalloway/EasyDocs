using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyDocs.ApiModels
{
    public class IndexStore
    {
        public string Title { get; set; }
        public int ID { get; set; }
        public string Type { get; set; }
        public string UrlKey { get; set; }
        public string SearchTerms { get; set; }
        public string Content { get; set; }
    }

    public class IndexResult 
    {
        public string Sample { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string UrlKey { get; set; }
    }
}