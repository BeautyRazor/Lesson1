using DevExpress.DashboardCommon;
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
    public class CRUDDashboardStorage : ICRUDDashboardStorage
    {
        private string workingDirectory;

        public CRUDDashboardStorage(string path)
        {
            workingDirectory = path;
        }

        private string GenerateID(string dashboardName)
        {
            string possibleDashboardID = ReplaceWrong(dashboardName);

            var dashboardIDs = GetAvaibleDashboardsID();

            var index = 0;
            var dashboardID = possibleDashboardID;

            while (dashboardIDs.Contains(dashboardID))
            {
                dashboardID = possibleDashboardID + (++index).ToString();
            }


            return dashboardID;
        }

        private List<string> GetAvaibleDashboardsID()
        {
            return Directory
                .GetFiles(workingDirectory, "*.xml")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        private string ReplaceWrong(string name)
        {
            foreach (var wrongChar in Path.GetInvalidFileNameChars())
                name = name.Replace(wrongChar.ToString(), string.Empty);
            return name;
        }

        private string GetNameByID(string dashboardID)
        {
            var xDashboard = LoadDashboard(dashboardID);
            var dashboard = new Dashboard();
            dashboard.LoadFromXDocument(xDashboard);

           return dashboard.Title.Text;
        }

        public string AddDashboard(XDocument xDashboard, string name)
        {
            var id = GenerateID(name);

            var dashboard = new Dashboard();
            dashboard.LoadFromXDocument(xDashboard);

            dashboard.Title.Text = name;

            dashboard.SaveToXDocument().Save(Path.Combine(workingDirectory, id + ".xml")); // COPY_PASTE!!!! Path.Combine(workingDirectory, id + ".xml"),  ".xml" - contant

            return id;
        }

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            return GetAvaibleDashboardsID()
                .Select(dasbhoardId => new DashboardInfo {
                    ID = dasbhoardId,
                    Name = GetNameByID(dasbhoardId)
                });
        }

        public XDocument LoadDashboard(string dashboardID) // cheack if file exists
        {
            if (dashboardID == null) return null;

            var file = Path.Combine(workingDirectory, dashboardID + ".xml");
            if (File.Exists(file))
            {
                return XDocument.Load(file);
            }
            else {
                return null;
            }
        }

        public void SaveDashboard(string dashboardID, XDocument dashboard)
        {
            //if (dashboardID == null) return null;

            dashboard.Save(Path.Combine(workingDirectory, dashboardID + ".xml"));
        }

        public string DeleteDashboard(string dashboardID) // this method should be void
        {
            if (dashboardID == null) return null;

            var file = Path.Combine(workingDirectory, dashboardID + ".xml");

            if (File.Exists(file))
            {
                File.Delete(file);
                return dashboardID;
            }
            else
            {
                return null;
            }

        }

        public string CloneDashboard(string dashboardID)
        {
            if (string.IsNullOrEmpty(dashboardID)) throw new ArgumentException("dashboardID cant be null");

            return CloneDashboard(dashboardID, "");
        }

        public string CloneDashboard(string dashboardID, string newDashboardName)
        {
            if (string.IsNullOrEmpty(dashboardID)) throw new ArgumentException("dashboardID cant be null");

            var document = LoadDashboard(dashboardID);

            if (document == null) return null;

            if (string.IsNullOrEmpty(newDashboardName)) {
                var dashboard = new Dashboard();
                dashboard.LoadFromXDocument(document);
                newDashboardName = dashboard.Title.Text;
            }
            return AddDashboard(document, newDashboardName);
        }
    }
}