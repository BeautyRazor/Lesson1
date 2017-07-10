using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
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
        static object lockObj = new object();

        public void Export(string thumbnailsPath, string dashboardID, string extension)
        {
            ASPxDashboardExporter exporter = new ASPxDashboardExporter(DashboardConfigurator.Default);

            var path = HostingEnvironment.MapPath(thumbnailsPath);

            lock (lockObj)
            {
                string fullPath = string.Format(@"{0}\{1}.{2}", path, dashboardID, extension);
                using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    switch (extension)
                    {
                        case "png":
                            exporter.ExportToImage(dashboardID, fs, new Size(512, 288), null, new DashboardImageExportOptions()
                            {
                                Format = DevExpress.DashboardCommon.DashboardExportImageFormat.Png
                            });
                            break;
                        case "jpg":
                        case "jpeg":
                            exporter.ExportToImage(dashboardID, fs, new Size(512, 288), null, new DashboardImageExportOptions()
                            {
                                Format = DevExpress.DashboardCommon.DashboardExportImageFormat.Jpeg
                            });
                            break;
                        case "gif":
                            exporter.ExportToImage(dashboardID, fs, new Size(512, 288), null, new DashboardImageExportOptions()
                            {
                                Format = DevExpress.DashboardCommon.DashboardExportImageFormat.Gif
                            });
                            break;
                        default:
                            Console.WriteLine("Wrong extension.");
                            break;

                    }
                }
            }
        }

    }
}