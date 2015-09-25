using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyDocs.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlKey { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string Content { get; set; }
        public string Draft { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int Order { get; set;}
        public string SideBarContent { get; set; }
        public User CreatedBy { get; set; }
        public User ModifiedBy { get; set; }


    }
}