using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public static class Dashboard
    {
        public static string Add()
        {
            string srcDashboard = HostingEnvironment.MapPath(@"~\App_Data\Resources\newdashboard.xml");
            var dashboardCount = new DirectoryInfo(HostingEnvironment.MapPath(@"~\App_Data\Dashboards")).GetFiles().Length;
            string destDashboard = HostingEnvironment.MapPath(@"~\App_Data\Dashboards\" + "dashboard" + (dashboardCount + 1).ToString() + ".xml");

            File.Copy(srcDashboard, destDashboard);

            return ("dashboard" + (dashboardCount + 1).ToString());
        }

        public static void Delete(string dashboardID)
        {
            string srcDashboard = HostingEnvironment.MapPath(@"~\App_Data\Dashboards\" + dashboardID + ".xml");
            File.Delete(srcDashboard);
        }
    }
}