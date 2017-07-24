using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default1",
                url: "",
                defaults: new { controller = "DashboardManager", action = "Preview" }
            );

            routes.MapRoute(
               name: "Add",
               url: "add/{name}",
               defaults: new { controller = "DashboardManager", action = "Add" }
           );

            routes.MapRoute(
                name: "Dashboard",
                url: "{id}/{action}",
                defaults: new { controller = "DashboardManager" }
            );
        }
    }
}
