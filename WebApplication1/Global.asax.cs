using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            DashboardConfig.RegisterService(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        public void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            MvcApplication mvcApplication = sender as MvcApplication;
            HttpRequest request = null;
            if (mvcApplication != null) request = mvcApplication.Request;
        }
    }
}
