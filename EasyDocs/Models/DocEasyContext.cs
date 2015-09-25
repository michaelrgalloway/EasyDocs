using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EasyDocs.Models
{
    public class DocEasyContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public DocEasyContext() : base("name=DocEasyContext")
        {
        }

        public System.Data.Entity.DbSet<EasyDocs.Models.Section> Sections { get; set; }
        public System.Data.Entity.DbSet<EasyDocs.Models.Page> Pages { get; set; }
        public System.Data.Entity.DbSet<EasyDocs.Models.Version> Versions { get; set; }
        public System.Data.Entity.DbSet<EasyDocs.Models.Download> Downloads { get; set; }
        public System.Data.Entity.DbSet<EasyDocs.Models.Article> Articles { get; set; }
        public System.Data.Entity.DbSet<EasyDocs.Models.ExternalLink> ExternalLinks { get; set; }
        public System.Data.Entity.DbSet<EasyDocs.Models.Settings> Settings { get; set; }
        public System.Data.Entity.DbSet<EasyDocs.Models.HeaderLink> HeaderLinks { get; set; }
        public System.Data.Entity.DbSet<EasyDocs.Models.User> Users { get; set; }

    }


}