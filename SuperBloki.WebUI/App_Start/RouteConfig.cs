using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SuperBloki.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Constructor",
                    action = "List",
                    producer = (string)null,
                    page = 1
                }
            );

            routes.MapRoute(
               name: null,
               url: "Page{page}",
               defaults: new { controller = "Constructor", action = "List", producer = (string)null },
               constraints: new { page = @"\d+" }
           );

            routes.MapRoute(null,
               "{producer}",
               new { controller = "Constructor", action = "List", page = 1 }
           );

            routes.MapRoute(null,
                "{producer}/Page{page}",
                new { controller = "Constructor", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");

        }
    }
}
