using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EasyDocs.Models;
using EasyDocs.ApiModels.ReadOnly;
using EasyDocs.ApiModels;

namespace EasyDocs.Controllers
{
    [RoutePrefix("api/pages")]
    public class PageController : BaseAPIController
    {
        [Route("PageHeaders")]
        public async Task<IHttpActionResult> GetPages()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var obj = this.GetFromCache<List<PageView>>(CacheKeys.PageList);
                if (obj != null)
                {
                    return Ok(obj);
                }
            }

            bool isAdmin = User.Identity.IsAuthenticated;
            List<PageView> retObj = await db.Pages.Where(w => w.Deleted != true && (w.Active == true || isAdmin == true)).Select(s => new PageView
            {
                Content = s.Content,
                Id = s.Id,
                Title = s.Title,
                UrlKey = s.UrlKey,
                Order = s.Order

            }).ToListAsync();

            if (!User.Identity.IsAuthenticated)
            {
                this.AddToCache<List<PageView>>(CacheKeys.PageList, retObj);
            }

            return Ok(retObj);
        }

        [Route("GetPage/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPage(string id)
        {
            if (id == "_home")
            {
                Settings settings = null;
                settings = this.GetFromCache<Settings>(CacheKeys.Settings);
                if (settings == null)
                {
                    settings = await db.Settings.FirstAsync();
                }

                id = settings.HomePage;
            }

            if (!User.Identity.IsAuthenticated)
            {
                var obj = this.GetFromCache<PageView>(CacheKeys.Page, id);
                if (obj != null)
                {
                    return Ok(obj);
                }
            }


            bool isAdmin = User.Identity.IsAuthenticated;
            var temp = await db.Pages.Where(w => w.UrlKey == id && w.Deleted != true && (w.Active == true || isAdmin == true)).FirstOrDefaultAsync();


            var page = new PageView();
            page.Content = temp.Content;
            page.Title = temp.Title;
            page.Id = temp.Id;
            page.UrlKey = temp.UrlKey;
            page.SideBarContent = temp.SideBarContent;

            if (User.Identity.IsAuthenticated)
            {
                page.Draft = temp.Draft;
                page.Active = temp.Active;
            }

            if (page == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                this.AddToCache<PageView>(CacheKeys.Page, page, id);
            }

            return Ok(page);
        }

        [Authorize]
        [Route("AddPage/{name}/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> AddPage(string name, int? id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return NotFound();
            }

            if(id!=null && id != 0)
            {
                Page existing = await db.Pages.FindAsync(id);
                if (existing == null)
                {
                    return NotFound();
                }
                existing.Title = name;

                db.Entry(existing).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Ok(existing);
            }

            Page page = new Page();
            page.Active = false;
            page.Content = "New Page Content Goes Here";
            page.Title = name;
            page.UrlKey = name.Replace(" ", "_").Trim();
            page.Created = DateTime.Now;
            page.Modified = DateTime.Now;

            var count = db.Pages.Where(w => w.UrlKey == page.UrlKey).Count();
            if(count > 0)
            {
                page.UrlKey = page.UrlKey + "_" + (count + 1).ToString();
            }

            var newPage = db.Pages.Add(page);
            await db.SaveChangesAsync();

            this.RemoveFromCache(CacheKeys.Page, page.UrlKey);
            this.RemoveFromCache(CacheKeys.PageList);

            return Ok(page);
        }

        [Authorize]
        [Route("SaveDraft/{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveDraft(int id, [FromBody]string draft)
        {
            if (id == 0)
            {
                return NotFound();
            }
          
            Page page = await db.Pages.Where(w => w.Id == id).FirstOrDefaultAsync();
            if (page == null)
            {
                return NotFound();
            }

            page.Draft = draft;
            db.Entry(page).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(true);
        }

        [Authorize]
        [Route("SaveSidebar/{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveSidebar(int id, [FromBody]string sidebar)
        {
            if (id == 0)
            {
                return NotFound();
            }

            Page page = await db.Pages.Where(w => w.Id == id).FirstOrDefaultAsync();
            if (page == null)
            {
                return NotFound();
            }

            page.SideBarContent = sidebar;
            db.Entry(page).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.RemoveFromCache(CacheKeys.Page, page.UrlKey);

            return Ok(true);
        }

        [Authorize]
        [Route("DeletePage/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> DeletePage(int id)
        {
            Page page = await db.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            page.Deleted = true;

            db.Entry(page).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.RemoveFromCache(CacheKeys.Page, page.UrlKey);
            this.RemoveFromCache(CacheKeys.PageList);
            return Ok(true);
        }

        [Authorize]
        [Route("PublishPage/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> PublishPage(int id)
        {
            Page page = await db.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            page.Content = page.Draft;
            page.Draft = null;

            db.Entry(page).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.RemoveFromCache(CacheKeys.Page, page.UrlKey);
            return Ok(true);
        }

        [Authorize]
        [Route("SaveUrlKey/{id}/{key}/{active}")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveUrlKey(int id, string key, bool active)
        {
            if (id == 0)
            {
                return NotFound();
            }

            Page page = await db.Pages.Where(w => w.Id == id).FirstOrDefaultAsync();
            if (page == null)
            {
                return NotFound();
            }
            this.RemoveFromCache(CacheKeys.Page, page.UrlKey);
            page.UrlKey = key;
            page.Active = active;
            db.Entry(page).State = EntityState.Modified;
            await db.SaveChangesAsync();
            
            this.RemoveFromCache(CacheKeys.PageList);
            return Ok(true);
        }

    }
}
