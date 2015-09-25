using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyDocs.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string LogoUrl { get; set; }
        public string HeaderText { get; set; }
        public List<HeaderLink> HeaderLinks { get; set; }
        public string HomePage { get; set; }
    }

    public class HeaderLink
    {
        public int Id { get; set; }
        public string Display { get; set; }
        public string Url { get; set; }
    }
}