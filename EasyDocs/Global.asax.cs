using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace EasyDocs
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //FormsAuthentication.LoginUrl
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteTable.Routes.IgnoreRoute("{file}.html");
            RouteTable.Routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            RouteTable.Routes.MapRoute(name: "login", url: "Login", defaults: new { controller = "Home", action = "Login" });
            RouteTable.Routes.MapRoute(name: "logout", url: "Logout", defaults: new { controller = "Home", action = "Logout" });
            RouteTable.Routes.MapRoute(name: "staticFileRoute", url: "{*file}", defaults: new { controller = "Home", action = "HandleStatic" });

        }

        void Application_EndRequest(object sender, System.EventArgs e)
        {
            // If the user is not authorised to see this page or access this function, send them to the login page.

            if (Response.StatusCode == 302 && Response.RedirectLocation.Contains("login.aspx"))
            {
                Response.ClearContent();
                Response.RedirectToRoute("login", (RouteTable.Routes["login"] as Route).Defaults);
            }
        }
    }
}
