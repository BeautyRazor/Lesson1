using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public static class Dashboard
    {
        public static string Add (string name)
        {
            var storage = (IEditableDashboardStorage)DashboardConfigurator.Default.DashboardStorage;

            var dashboard = new DevExpress.DashboardCommon.Dashboard();
            string id = storage.AddDashboard(dashboard.SaveToXDocument(), name);

            return id;
        }

        public static string Delete (string id)
        {
            string srcDashboard = HostingEnvironment.MapPath(@"~\App_Data\Dashboards\" + id + ".xml");
            File.Delete(srcDashboard);

            return id;
        }

        public static string Clone (string id)
        {
            var storage = (IEditableDashboardStorage)DashboardConfigurator.Default.DashboardStorage;

            var dashboard = storage.LoadDashboard(id);
            var name = dashboard.Root.Element("Title").Attribute("Text").Value;

            return storage.AddDashboard(dashboard, name); ;
        }

        public static string Clone(string id, string name)
        {
            var storage = (IEditableDashboardStorage)DashboardConfigurator.Default.DashboardStorage;

            var dashboard = storage.LoadDashboard(id);

            return storage.AddDashboard(dashboard, name); ;
        }


        private static string ReplaceWrong(string name) //for future possible self database
        {
            string wrongChars = "/\\*:?| \"<>";

            foreach (var wrong in wrongChars)
                name.Replace(wrong, '_');

            return name;
        }
             
    }
}