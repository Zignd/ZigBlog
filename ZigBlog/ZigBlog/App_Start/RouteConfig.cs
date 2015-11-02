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
                name: "User",
                url: "{action}",
                defaults: new { controller = "User" }
            );

            routes.MapRoute(
                name: "Page",
                url: "Page/{number}",
                defaults: new { controller = "Home", action = "Page" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Page" }
            );
        }
    }
}
