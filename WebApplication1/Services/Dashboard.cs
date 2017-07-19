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
    public class Dashboard : DashboardFileStorage
    {
        private string dasboardID;

        public Dashboard(string id)
        {
            dasboardID = id;
        }

        public Dashboard() { }

        public string Add (string name)
        {
            //string srcDashboard = HostingEnvironment.MapPath(@"~\App_Data\Dashboards\");

            var storage = (IEditableDashboardStorage)DashboardConfigurator.Default.DashboardStorage;
            //var storage = new Services.MyDashboardFileStorage(srcDashboard);

            var dashboard = new DevExpress.DashboardCommon.Dashboard();
            string id = storage.AddDashboard(dashboard.SaveToXDocument(), name);

            return id;
        }

        public void Delete()
        {
            string srcDashboard = HostingEnvironment.MapPath(@"~\App_Data\Dashboards\" + dasboardID + ".xml");
            File.Delete(srcDashboard);
        }

        public string Clone()
        {
            var storage = (IEditableDashboardStorage)DashboardConfigurator.Default.DashboardStorage;

            var dashboardXML = storage.LoadDashboard(dasboardID);
            var dashboard = new DevExpress.DashboardCommon.Dashboard();
            dashboard.LoadFromXDocument(dashboardXML);

            var name = dashboard.Title.Text;

            var id = storage.AddDashboard(dashboardXML, name);
            return id ;
        }

        public string Clone(string name)
        {
            var storage = (IEditableDashboardStorage)DashboardConfigurator.Default.DashboardStorage;

            var dashboard = storage.LoadDashboard(dasboardID);

            return storage.AddDashboard(dashboard, name); ;
        }


       
             
    }
}