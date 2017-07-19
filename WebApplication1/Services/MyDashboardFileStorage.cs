using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;

namespace WebApplication1.Services
{
    public class MyDashboardFileStorage : IEditableDashboardStorage  // rename class name
    {
        public string WorkingDirectory; //private fields should be named with small letter
        // DO NOT MAKE PUBLIC FIELDS --- NEVER
        public MyDashboardFileStorage(string path)
        {
            WorkingDirectory = HostingEnvironment.MapPath(path);
        }

        public string AddDashboard(XDocument dashboard, string name)
        {
            var id = GenerateID(name);
            dashboard.Root.Element("Title").Attribute("Text").Value = name; // DO it with Dashboard class

            dashboard.Save(Path.Combine(WorkingDirectory, id + ".xml"));
           
            return id;
        }

        public List<string> GetAvaibleDashboardsID() // make it private
        {
            var ids = new List<string>();
            ids = Directory.GetFiles(WorkingDirectory, "*.xml").ToList();

            return ids;
        }
        public string GenerateID(string name) // do not make public method only for tests!!!
        {
            string id = ReplaceWrong(name); // synchronize local var names ("id" and "name")

            var dashboards = GetAvaibleDashboardsID();
            // =======
            var i = 0;
            var dashboardName = id;
            while (dashboards.Contains(dashboardName)) {
                dashboardName = id  + (++i).ToString();
            }

            id = dashboardName;

            // =======
            //bool isContain = true;
            //int i = 0;
            //var buf = id;

            //while (isContain)
            //{
            //    isContain = false;

            //    if (i != 0)
            //        buf = id + "_" + i.ToString();


            //    foreach (var item in dashboards)
            //        if (item == buf)
            //        {
            //            isContain = true;
            //            buf = id + "_" + (++i).ToString();
            //        }
            //}

            //id = buf;


            return id;
        }

        public string ReplaceWrong(string name)  // do not make public method only for tests!!!
        {
            string wrongChars = "/\\*:?| \"<>_";// you can use Path.GetInvalidFileNameChars()
            foreach (var wrong in wrongChars)
                name = name.Replace(wrong, '-');

            return name;
        }

      
        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            var ids = GetAvaibleDashboardsID();
            var info = new List<DashboardInfo>();

            foreach (var item in ids) // fu fu fu. DO not you "for" when you can use LINQ
            {
                info.Add(new DashboardInfo
                {
                    ID = item,
                    Name = item
                });
            }


            return info;
        }

        public XDocument LoadDashboard(string dashboardID) // cheack if file exists
        {
            return XDocument.Load(Path.Combine(WorkingDirectory, dashboardID + ".xml"));
        }

        public void SaveDashboard(string dashboardID, XDocument dashboard)
        {
            dashboard.Save(Path.Combine(WorkingDirectory, dashboardID + ".xml"));
        }
    }
}