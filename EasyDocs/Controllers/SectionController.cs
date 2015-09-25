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
    [RoutePrefix("api/sections")]
    public class SectionsController : BaseAPIController
    {
       
        [Route("SectionHeaders")]
        public async Task<IHttpActionResult> GetSections()
        {
            ///Section/Api_Module_1

          

            if (!User.Identity.IsAuthenticated)
            {
                var obj = this.GetFromCache<List<SectionView>>(CacheKeys.SectionList);
                if (obj != null)
                {
                    return Ok(obj);
                }
            }

            List<SectionView> retObj = await getChildSections(null, null);

            if (!User.Identity.IsAuthenticated)
            {
                this.AddToCache<List<SectionView>>(CacheKeys.SectionList, retObj);
            }

            return Ok(retObj);
        }

        //private List<SectionView> applyExpanded(List<SectionView> list, string key)
        //{
        //    if (key == string.Empty) return list;
        //    foreach(var s in list)
        //    {
        //       if(s.Sections.Any(a=>a.UrlKey.ToLower() == key))
        //        {
        //            s.Expanded = true;
        //        }
        //        else
        //        {
        //           s.Sections =  applyExpanded(s.Sections, key);
        //        }
        //    }
        //    return list;
        //}

        private async Task<List<SectionView>> getChildSections(int? sectionID, int? count)
        {
            bool isAdmin = User.Identity.IsAuthenticated;

            List<SectionView> retObj = new List<SectionView>();

            if (count == 0) return retObj;

            var temp = await db.Sections.Where(w => w.ParentID == sectionID && w.Deleted != true && (w.Active == true || isAdmin == true))
           .Select(s => new
           {
              // Content = s.Contents,
               ID = s.Id,
               Title = s.Title,
               Count = s.Sections.Count(),
               UrlKey = s.URLKey
           })
           .ToListAsync();
            foreach (var s in temp)
            {
                retObj.Add(new SectionView()
                {
                 //   Content = s.Content,
                    ID = s.ID,
                    Title = s.Title,
                    Sections = await getChildSections(s.ID, s.Count),
                    UrlKey = s.UrlKey

                });
            }

            return retObj;
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetSection(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var obj = this.GetFromCache<SectionView>(CacheKeys.Section,id);
                if (obj != null)
                {
                    return Ok(obj);
                }
            }

            bool isAdmin = User.Identity.IsAuthenticated;
            var s = await db.Sections.Include("Downloads").Include("ExternalLinks").Include("Articles").Where(w => w.URLKey == id && w.Deleted != true && (w.Active == true || isAdmin == true)).FirstOrDefaultAsync();

            SectionView section = new SectionView();
            section.Content = s.Contents;
            section.ID = s.Id;
            section.Title = s.Title;
            section.UrlKey = s.URLKey;
            section.Articles = s.Articles.Select(ss => new Article() { Id = ss.Id, Title = ss.Title, Url = ss.Url}).ToList();
            section.Downloads = s.Downloads.Select(ss => new Download() { Id = ss.Id, Title = ss.Title, Url = ss.Url }).ToList();
            section.ExternalLinks = s.ExternalLinks.Select(ss => new ExternalLink() { Id = ss.Id, Title = ss.Title, Url = ss.Url }).ToList();

            if (User.Identity.IsAuthenticated)
            {
                section.Draft = s.Draft;
                section.Active = s.Active;
            }

            if (section == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                this.AddToCache<SectionView>(CacheKeys.Section, section,id);
            }

            return Ok(section);
        }

        [Authorize]
        [Route("AddSection/{name}/{id}/{pid}")]
        [HttpGet]
        public async Task<IHttpActionResult> AddSection(string name,int id, int?pid)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return NotFound();
            }

            if (id != 0)
            {
                Section existing = await db.Sections.FindAsync(id);
                if (existing == null)
                {
                    return NotFound();
                }
                existing.Title = name;

                db.Entry(existing).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Ok(existing);
            }

            int userId = int.Parse(this.User.Identity.Name);
            var user = await db.Users.FindAsync(userId);

            Section section = new Section();
            section.ParentID = (pid == 0 ? null : pid);
            section.Active = false;
            section.Contents = name;
            section.Title = name;
            section.URLKey = name.Replace(" ", "_").Trim();
            section.VersionId = 1;
            section.Created = DateTime.UtcNow;
            section.CreatedBy = user;
            section.Modified = DateTime.UtcNow;
            section.ModifiedBy = user;

            var count = db.Sections.Where(w => w.URLKey == section.URLKey).Count();
            if (count > 0)
            {
                section.URLKey = section.URLKey + "_" + (count + 1).ToString();
            }


            var newSection = db.Sections.Add(section);
            await db.SaveChangesAsync();
            this.RemoveFromCache(CacheKeys.Section, section.URLKey);
            this.RemoveFromCache(CacheKeys.SectionList);
            return Ok(newSection);
        }

        [Authorize]
        [Route("DeleteSection/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> DeleteSection(int id)
        {
            Section section = await db.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            section.Deleted = true;

            db.Entry(section).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.RemoveFromCache(CacheKeys.Section, section.URLKey);
            this.RemoveFromCache(CacheKeys.SectionList);
            return Ok(true);
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

            Section section = await db.Sections.Where(w => w.Id == id).FirstOrDefaultAsync();
            if (section == null)
            {
                return NotFound();
            }

            section.Draft = draft;
            db.Entry(section).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(true);
        }

        [Authorize]
        [Route("SaveResoures")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveResoures(SectionView data)
        {
           // SectionView data = new SectionView();

            if (data.ID == 0)
            {
                return NotFound();
            }

            Section section = await db.Sections.Include("Downloads").Include("ExternalLinks").Include("Articles").Where(w => w.Id == data.ID).FirstOrDefaultAsync();
            if (section == null)
            {
                return NotFound();
            }

            //Downloads//////////////////////////////////////////////
            foreach (var d in section.Downloads.ToList())
            {

                var temp = data.Downloads.Where(w => w.Id == d.Id).FirstOrDefault();
                if (temp != null)
                {
                    d.Title = temp.Title;
                    d.Url = temp.Url;
                    db.Entry(d).State = EntityState.Modified;

                }
                else
                {
                    db.Entry(d).State = EntityState.Deleted;
                }
            }

            foreach (var d in data.Downloads.Where(w => w.Id == 0).ToList())
            {
                Download dl = new Download();
                dl.Title = d.Title;
                dl.Url = d.Url;
                section.Downloads.Add(dl);
                db.Downloads.Add(dl);
            }
            ////////////////////////////////////////////////////////
            //Articles//////////////////////////////////////////////
            foreach (var d in section.Articles.ToList())
            {

                var temp = data.Articles.Where(w => w.Id == d.Id).FirstOrDefault();
                if (temp != null)
                {
                    d.Title = temp.Title;
                    d.Url = temp.Url;
                    db.Entry(d).State = EntityState.Modified;

                }
                else
                {
                    db.Entry(d).State = EntityState.Deleted;
                }
            }

            foreach (var d in data.Articles.Where(w => w.Id == 0).ToList())
            {
                Article dl = new Article();
                dl.Title = d.Title;
                dl.Url = d.Url;
                section.Articles.Add(dl);
                db.Articles.Add(dl);
            }
            ////////////////////////////////////////////////////////
            //Downloads//////////////////////////////////////////////
            foreach (var d in section.ExternalLinks.ToList())
            {

                var temp = data.ExternalLinks.Where(w => w.Id == d.Id).FirstOrDefault();
                if (temp != null)
                {
                    d.Title = temp.Title;
                    d.Url = temp.Url;
                    db.Entry(d).State = EntityState.Modified;

                }
                else
                {
                    db.Entry(d).State = EntityState.Deleted;
                }
            }

            foreach (var d in data.ExternalLinks.Where(w => w.Id == 0).ToList())
            {
                ExternalLink dl = new ExternalLink();
                dl.Title = d.Title;
                dl.Url = d.Url;
                section.ExternalLinks.Add(dl);
                db.ExternalLinks.Add(dl);
            }
            ////////////////////////////////////////////////////////
            db.Entry(section).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.RemoveFromCache(CacheKeys.Section, section.URLKey);
            return Ok(true);
        }

        [Authorize]
        [Route("SaveUrlKey/{id}/{key}/{active}")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveUrlKey(int id, string key,bool active)
        {
            if (id == 0)
            {
                return NotFound();
            }

            Section section = await db.Sections.Where(w => w.Id == id).FirstOrDefaultAsync();
            if (section == null)
            {
                return NotFound();
            }
            this.RemoveFromCache(CacheKeys.Section, section.URLKey);
            section.URLKey = key;
            section.Active = active;
            db.Entry(section).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.RemoveFromCache(CacheKeys.SectionList);
            return Ok(true);
        }

        [Authorize]
        [Route("Publish/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Publish(int id)
        {
            Section section = await db.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            section.Contents = section.Draft;
            section.Draft = null;

            db.Entry(section).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.RemoveFromCache(CacheKeys.Section, section.URLKey);
            return Ok(true);
        }


        private bool SectionExists(int id)
        {
            return db.Sections.Count(e => e.Id == id) > 0;
        }
    }
}