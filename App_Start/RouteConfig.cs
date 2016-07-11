using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ThrowdownAttire
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Shirt",
                url: "Shirt/{id}",
                defaults: new { controller = "Home", action = "Shirt" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Auth",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Auth", action = "AdminLogin", id = UrlParameter.Optional }
                );
        }
    }
}
