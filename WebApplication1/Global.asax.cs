using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevExpress.DashboardCommon;
using DevExpress.Logify.Web;

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

            DevExpress.Utils.AzureCompatibility.Enable = true;
            DashboardExportSettings.CompatibilityMode = DashboardExportCompatibilityMode.Restricted;

        }

        public void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            LogifyAlert client = LogifyAlert.Instance;

            client.ApiKey = "6A85A8CCB57E4F14945DCDC36EF1B49E";

            client.Send(exc);
        }
    }
}
