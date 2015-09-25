using EasyDocs.ApiModels;
using EasyDocs.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyDocs.Controllers
{
    [RoutePrefix("api/search")]
    public class SearchController : BaseAPIController
    {
        [Authorize]
        [HttpGet]
        [Route("Rebuild")]
        public async Task<IHttpActionResult> Rebuild()
        {
            var pages = await db.Pages.Where(w => w.Deleted != true && w.Active == true)
                .Select(s => new IndexStore()
                {
                    Content = s.Content,
                    ID = s.Id,
                    SearchTerms = "",
                    Title = s.Title,
                    Type = "page",
                    UrlKey = s.UrlKey
                }).ToListAsync();

            var sections = await db.Sections.Where(w => w.Deleted != true && w.Active == true)
                .Select(s => new IndexStore()
                {
                    Content = s.Contents,
                    ID = s.Id,
                    SearchTerms = s.SearchTerms,
                    Title = s.Title,
                    Type = "section",
                    UrlKey = s.URLKey
                }).ToListAsync();

            sections.AddRange(pages);

            LuceneHelper indexHelper = new LuceneHelper();
            indexHelper.PopulateIndex(sections);

            return Ok();
        }



        [HttpGet]
        [Route("Search/{terms}")]
        public IHttpActionResult Search(string terms)
        {
            LuceneHelper indexHelper = new LuceneHelper();
            var search = indexHelper.Search(terms);
            return Ok(search);
        }
    }

}
