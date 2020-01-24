using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DevTestProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Employees",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "EmployeesController", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Teams",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TeamsController", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Projects",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "ProjectsController", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
