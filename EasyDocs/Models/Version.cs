using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyDocs.Models
{
    public class Version
    {
        public int Id { get; set; }
        [Required]
        public string Desc { get; set; }
    }
}