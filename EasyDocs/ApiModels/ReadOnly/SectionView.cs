using EasyDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyDocs.ApiModels.ReadOnly
{
    public class SectionView
    {
        public SectionView()
        {
            this.Expanded = false;
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<SectionView> Sections { get; set; }
        public string UrlKey { get; set; }
        public string Contents { get; set; }
        public string Draft { get; set; }
        public bool Active { get; set; }
        public bool Expanded { get; set; }

        public List<Download> Downloads { get; set; }
        public List<ExternalLink> ExternalLinks { get; set; }
        public List<Article> Articles { get; set; }
    }
}