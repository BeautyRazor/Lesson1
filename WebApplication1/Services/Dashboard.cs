using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public static class Dashboard
    {
        public static string Add(string name)
        {
            string srcDashboard = HostingEnvironment.MapPath(@"~\App_Data\Resources\newdashboard.xml");
            var dashboardCount = new DirectoryInfo(HostingEnvironment.MapPath(@"~\App_Data\Dashboards")).GetFiles().Length;
            string destDashboard = HostingEnvironment.MapPath(@"~\App_Data\Dashboards\" + "dashboard" + (dashboardCount + 1).ToString() + ".xml");

            var storage = new DashboardFileStorage(@"~\App_Data\Dashboards");

            var edit = (storage as IEditableDashboardStorage).GetAvailableDashboardsInfo().ToList();
            string wrongChars = "/\\*:?| \"<>";

            foreach(var wrong in wrongChars)
                name.Replace(wrong, '_');

            var config = new DashboardInfo()
            {
                ID = name,
                Name = name
            };

            edit.Add(config);

            //XmlDocument doc = new XmlDocument();

            //doc.Load(srcDashboard);

            //XmlNode root = doc.DocumentElement;
            //XmlNode myNode = root.SelectSingleNode("Title");
            //var attr = myNode.Attributes["Text"];
            //attr.Value = name;

            //doc.Save(destDashboard);

            return config.ID;
        }

        public static void Delete(string dashboardID)
        {
            string srcDashboard = HostingEnvironment.MapPath(@"~\App_Data\Dashboards\" + dashboardID + ".xml");
            File.Delete(srcDashboard);
        }
    }
}