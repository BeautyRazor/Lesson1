using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;

namespace WebApplication1.Services
{
    public class CrudDashboardStorage : ICrudDashboardStorage
    {
        private readonly string _workingDirectory;
        private const string Extention = ".xml";

        public CrudDashboardStorage(string path)
        {
            _workingDirectory = path;
        }

        string GetFilePath(string id)
        {
            return Path.Combine(_workingDirectory, id + Extention);
        }

        private string GenerateId(string dashboardName)
        {
            var possibleDashboardId = ReplaceWrong(dashboardName);

            var dashboardIDs = GetAvaibleDashboardsId();

            var index = 0;
            var dashboardId = possibleDashboardId;

            while (dashboardIDs.Contains(dashboardId))
            {
                dashboardId = possibleDashboardId + (++index);
            }


            return dashboardId;
        }

        private List<string> GetAvaibleDashboardsId()
        {
            return Directory
                .GetFiles(_workingDirectory, "*" + Extention)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        private static string ReplaceWrong(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(
                name, 
                (current, wrongChar) => current.Replace(wrongChar.ToString(), 
                string.Empty));
        }

        private string GetNameById(string dashboardId)
        {
            var xDashboard = LoadDashboard(dashboardId);
            var dashboard = new Dashboard();
            dashboard.LoadFromXDocument(xDashboard);

           return dashboard.Title.Text;
        }

        public string AddDashboard(XDocument xDashboard, string name)
        {
            var id = GenerateId(name);

            var dashboard = new Dashboard();
            dashboard.LoadFromXDocument(xDashboard);

            dashboard.Title.Text = name;

            dashboard.SaveToXDocument().Save(GetFilePath(id)); 

            return id;
        }

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            return GetAvaibleDashboardsId()
                .Select(dasbhoardId => new DashboardInfo {
                    ID = dasbhoardId,
                    Name = GetNameById(dasbhoardId)
                });
        }

        public XDocument LoadDashboard(string dashboardId) // cheack if file exists
        {
            if (string.IsNullOrEmpty(dashboardId)) throw new ArgumentException("dashboardID cant be null");

            var file = GetFilePath(dashboardId);
            if (File.Exists(file))
            {
                return XDocument.Load(file);
            }
            throw new FileNotFoundException("No dashboard with this id found");
        }

        public void SaveDashboard(string dashboardId, XDocument dashboard)
        {
            if (string.IsNullOrEmpty(dashboardId)) throw new ArgumentException("dashboardID cant be null");
            if (dashboard == null) throw new ArgumentException("dashboardXML cant be null");

            dashboard.Save(Path.Combine(_workingDirectory, dashboardId + ".xml"));
        }

        public string DeleteDashboard(string dashboardId) 
        {
            if (string.IsNullOrEmpty(dashboardId)) throw new ArgumentException("dashboardID cant be null");

            var file = GetFilePath(dashboardId);

            if (!File.Exists(file))
                throw new FileNotFoundException("No dashboard with this id found");

            File.Delete(file);
            return dashboardId;
        }

        public string CloneDashboard(string dashboardId)
        {
            if (string.IsNullOrEmpty(dashboardId)) throw new ArgumentException("dashboardID cant be null");

            return CloneDashboard(dashboardId, "");
        }

        public string CloneDashboard(string dashboardId, string newDashboardName)
        {
            if (string.IsNullOrEmpty(dashboardId)) throw new ArgumentException("dashboardID cant be null");

            var document = LoadDashboard(dashboardId);

            if (document == null) throw new FileNotFoundException("No dashboard with this id found");

            if (!string.IsNullOrEmpty(newDashboardName)) return AddDashboard(document, newDashboardName);

            var dashboard = new Dashboard();
            dashboard.LoadFromXDocument(document);
            newDashboardName = dashboard.Title.Text;

            return AddDashboard(document, newDashboardName);
        }
    }
}