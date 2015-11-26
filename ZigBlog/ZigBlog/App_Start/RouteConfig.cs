using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZigBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;
            
            routes.MapRoute(
                name: "Edit",
                url: "{year}/{month}/{day}/{titleUrl}/edit",
                defaults: new { controller = "Home", action = "Edit" });

            routes.MapRoute(
                name: "Show",
                url: "{year}/{month}/{day}/{titleUrl}",
                defaults: new { controller = "Home", action = "Show" });

            routes.MapRoute(
                name: "Profile",
                url: "profile/{userName}",
                defaults: new { controller = "User", action = "Profile" });

            routes.MapRoute(
                name: "New",
                url: "new",
                defaults: new { controller = "Home", action = "New" });

            routes.MapRoute(
                name: "HomePage",
                url: "page/{page}/{postsPerPage}",
                defaults: new { controller = "Home", action = "Page", postsPerPage = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Page" }
            );
        }
    }
}
