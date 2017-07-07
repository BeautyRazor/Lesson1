using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebApplication1.Services
{
    public class StreamTest
    {
        private void DeleteUnusefulFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        public List<long> Export(DashboardFileStorage dashboardStorage, string thumbnailsPath)
        { 
            ASPxDashboardExporter exporter = new ASPxDashboardExporter(DashboardConfigurator.Default);

            var path = HostingEnvironment.MapPath(thumbnailsPath);
            var dashboards = (dashboardStorage as IDashboardStorage).GetAvailableDashboardsInfo().ToList();

            DeleteUnusefulFiles(path);

            List<long> streamList = new List<long>();

            for (int i = 0; i < dashboards.Count; i++)
            {
                string fullPath = string.Format(@"{0}\{1}.png", path, dashboards[i].ID);
                using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    exporter.ExportToImage(dashboards[i].ID, fs, new Size(500, 500), null, new DashboardImageExportOptions()
                    {
                        Format = DevExpress.DashboardCommon.DashboardExportImageFormat.Png
                    });
                    streamList.Add(fs.Length);
                }
            }

            return streamList;
        }

    }
}