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
using System.Collections.Generic;
using EasyDocs.ViewModels;

namespace EasyDocs.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : BaseAPIController
    {

        [Authorize]
        [Route("GetUsers")]
        public async Task<IHttpActionResult> GetUsers()
        {
            var retObj = await db.Users.Select(s =>
            new
            {
                Email = s.Email,
                Active = s.Active,
                Id = s.Id

            }).ToListAsync();
            return Ok(retObj);
        }

        [Authorize]
        [Route("AddUser")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUser(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                return NotFound();
            }

            User existing = await db.Users.Where(w => w.Email == model.UserName).FirstOrDefaultAsync();
            if (existing != null)
            {
                return BadRequest();
            }



            string hashPass = BCrypt.Net.BCrypt.HashPassword(model.Password, BCrypt.Net.BCrypt.GenerateSalt());
            User user = new User();
            user.Active = true;
            user.Email = model.UserName;
            user.Password = hashPass;
            user.CreatedDate = System.DateTime.UtcNow;

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [Authorize]
        [Route("ToggleUser/{id}/{active}")]
        [HttpPost]
        public async Task<IHttpActionResult> ToggleUser(int id, bool active)
        {


            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            user.Active = active;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(active);
        }

       
    }
}
