using EasyDocs.Models;
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
using System.Web;
using System.IO;
using EasyDocs.ApiModels;

namespace EasyDocs.Controllers
{
    [RoutePrefix("api/settings")]
    public class SettingsController : BaseAPIController
    {
        

        [Route("GetSettings")]
        public async Task<IHttpActionResult> GetSettings()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var obj = this.GetFromCache<Settings>(CacheKeys.Settings);
                if (obj != null)
                {
                    return Ok(obj);
                }
            }

            Settings retObj = await db.Settings.Include("HeaderLinks").FirstOrDefaultAsync();

            if (!User.Identity.IsAuthenticated)
            {
                this.AddToCache<Settings>(CacheKeys.Settings, retObj);
            }

            return Ok(retObj);
        }

        [Authorize]
        [Route("SaveSettings")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveSettings(Settings settings)
        {
            if (settings == null)
            {
                return BadRequest();
            }
            Settings settingsObj = await db.Settings.Include("HeaderLinks").FirstOrDefaultAsync();
            settingsObj.HeaderText = settings.HeaderText;
            settingsObj.LogoUrl = settings.LogoUrl;
            settingsObj.HomePage = settings.HomePage;

            foreach (var h in settingsObj.HeaderLinks.ToList())
            {
                
                var temp = settings.HeaderLinks.Where(w => w.Id == h.Id).FirstOrDefault();
                if (temp != null)
                {
                    h.Display = temp.Display;
                    h.Url = temp.Url;
                    db.Entry(h).State = EntityState.Modified;

                }
                else
                {
                    db.Entry(h).State = EntityState.Deleted;
                }
            }

            foreach(var h in settings.HeaderLinks.Where(w=>w.Id == 0).ToList())
            {
                HeaderLink hl = new HeaderLink();
                hl.Display = h.Display;
                hl.Url = h.Url;
                settingsObj.HeaderLinks.Add(hl);
                db.HeaderLinks.Add(hl);
            }

            db.Entry(settingsObj).State = EntityState.Modified;
            await db.SaveChangesAsync();

            this.RemoveFromCache(CacheKeys.Settings);
            return Ok(true);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        [HttpPost, Route("UploadFile")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            string returnFileName = string.Empty;
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                var folder  = HttpContext.Current.Server.MapPath("~/web/assets/img");
                File.WriteAllBytes(Path.Combine(folder, filename), buffer);
                returnFileName = "/web/assets/img/" + filename;
            }
            if (string.IsNullOrWhiteSpace(returnFileName)){
                return BadRequest();
            }
            return Ok(returnFileName);
        }
    }
}
