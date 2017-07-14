using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DXWebApplication1 {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{action}/{id}",
                defaults: new { controller = "WebAPI", id = RouteParameter.Optional }
            );
        }
    }
}