using System;
using System.Drawing;
using System.IO;
using System.Web.Hosting;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DashboardExportImageFormat = DevExpress.DashboardCommon.DashboardExportImageFormat;

namespace WebApplication1.Services
{
    public class DashbordExporter
    {
        private void DeleteUnusefulFiles(string path)
        {
            var di = new DirectoryInfo(path);
            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
        }
        static object _lockObj = new object();

        public void Export(string thumbnailsPath, string dashboardId, string extension, string hash)
        {
            var exporter = new ASPxDashboardExporter(DashboardConfigurator.Default);

            var path = thumbnailsPath;

            lock (_lockObj)
            {
                var fullPath = string.Format(@"{0}\{1}_{2}.{3}", path, dashboardId, hash, extension);
                using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    switch (extension)
                    {
                        case "png":
                            exporter.ExportToImage(dashboardId, fs, new Size(512, 288), null, new DashboardImageExportOptions
                            {
                                Format = DashboardExportImageFormat.Png
                            });
                            break;
                        case "jpg":
                        case "jpeg":
                            exporter.ExportToImage(dashboardId, fs, new Size(512, 288), null, new DashboardImageExportOptions
                            {
                                Format = DashboardExportImageFormat.Jpeg
                            });
                            break;
                        case "gif":
                            exporter.ExportToImage(dashboardId, fs, new Size(512, 288), null, new DashboardImageExportOptions
                            {
                                Format = DashboardExportImageFormat.Gif
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