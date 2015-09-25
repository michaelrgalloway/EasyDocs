using EasyDocs.Models;
using EasyDocs.ViewModels;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EasyDocs.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult HandleStatic()
        {
            if (string.IsNullOrWhiteSpace(Request.CurrentExecutionFilePathExtension))
            {
                if (Request.Path.StartsWith("/admin"))
                {
                    return File(string.Format("~/admin/index.html"), "text/HTML");
                }
                else
                {
                    return File(string.Format("~/web/index.html"), "text/HTML");
                }
            }

            return HttpNotFound();

            

            //if (User.Identity.IsAuthenticated)
            //{
            //    var location = this.Request.Url.PathAndQuery;
            //    if(this.Request.Path == @"/")
            //    {
            //        location = "index.html";
            //    }
            //    return File(string.Format("~/{0}", this.Request.Url.PathAndQuery), "text/HTML");
            //}
            //else
            //{
            //    var location = this.Request.Url.PathAndQuery;
            //    if(this.Request.Path == @"/")
            //    {
            //        location = "index.html";
            //    }
            //    return File(string.Format("~/web/{0}", location), "text/HTML");
            //}

        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ViewBag.error = "Username is required";
                ViewBag.username = model.UserName;
                return View();
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.error = "Password is required";
                ViewBag.username = model.UserName;
                return View();
            }

            using(DocEasyContext db = new DocEasyContext())
            {
                var user = db.Users.Where(w =>
               w.Active == true &&
               w.Email == model.UserName ).FirstOrDefault();

                if(user == null)
                {
                    ViewBag.error = "Invalid username or password";
                    ViewBag.username = model.UserName;
                    return View();
                }
                else
                {
                    if(BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                    {
                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                        return Redirect("/admin");
                    }
                    else
                    {
                        ViewBag.error = "Invalid username or password";
                        ViewBag.username = model.UserName;
                        return View();
                    }
                }
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return Redirect("/");
        }
    }
}