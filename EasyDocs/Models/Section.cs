using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EasyDocs.Models
{
    public class Section
    {
        public int Id { get; set; }
        [Required]
        public string URLKey { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }
        public int? ParentID { get; set; }
        public string SearchTerms { get; set; }
        public bool Active { get; set; }
        public Version Version { get; set; }
        public int VersionId { get; set; }
        [ForeignKey("ParentID")]
        public virtual ICollection<Section> Sections { get; set; }
        public int? Order { get; set; }
        public bool? Deleted { get; set; }
        public string Draft { get; set; }
        public List<Download> Downloads { get; set; }
        public List<ExternalLink> ExternalLinks { get; set; }
        public List<Article> Articles { get; set; }
        public User CreatedBy { get; set; }
        public User ModifiedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified {get;set;}

    }
}