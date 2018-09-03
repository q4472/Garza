using System.Web.Mvc;
using System.Web.Routing;

namespace Garza.MvcApplication2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "{*pathInfo}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}