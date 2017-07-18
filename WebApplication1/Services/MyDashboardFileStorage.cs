using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WebApplication1.Services
{
    public class MyDashboardFileStorage : IEditableDashboardStorage
    {
        public string WorkingDirectory;
        public MyDashboardFileStorage(string path)
        {
            WorkingDirectory = path;
        }

        public string AddDashboard(string dashboardName)
        {
            var dashboard = new DevExpress.DashboardCommon.Dashboard();
            var id = GenerateID(dashboardName);
            dashboard.Title.Text = dashboardName;

            dashboard.SaveToXml(Path.Combine(WorkingDirectory, id + ".xml"));
           
            return id;
        }

        public List<string> GetAvaibleDashboardID()
        {
            var ids = new List<string>();
            ids.

            return ids;
        }
        public string GenerateID(string name)
        {
            string id = ReplaceWrong(name);

            var dashboards = GetAvailableDashboardsID();
            // =======
            var i = 0;
            var dashbordName = id;
            while (dashboards.Contains(dashbordName)) {
                dashbordName = id  + (++i).ToString();
            }

            id = dashbordName;

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

        public string ReplaceWrong(string name) //for future possible self database
        {
            string wrongChars = "/\\*:?| \"<>_";
            foreach (var wrong in wrongChars)
                name = name.Replace(wrong, '-');

            return name;
        }
    }
}