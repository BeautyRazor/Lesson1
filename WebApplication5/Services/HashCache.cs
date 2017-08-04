
using System.Drawing;
using DevExpress.DashboardWeb;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using DevExpress.DataProcessing.InMemoryDataProcessor.Executors;

namespace WebApplication5.Services
{
    public class HashCache
    {
        public string CacheRequest(string thumbnailsPath, string dashboardPath, string dashboardId, string dashboardFileExtention)
        {
            var newFile = dashboardPath + dashboardId + "." + "xml";

            var sha256 = SHA256.Create().ComputeHash(File.ReadAllBytes(newFile));
            var hash = "";

            foreach (var h in sha256)
                hash += h.ToString("x2");


            newFile = thumbnailsPath + dashboardId + "_" + hash + "." + dashboardFileExtention;
            
            if (!File.Exists(newFile)) {
                var directory = new DirectoryInfo(thumbnailsPath).GetFiles();

                foreach (var file in directory)
                {
                  if (file.Name.Contains(dashboardId + "_"))
                    {
                        File.Delete(file.FullName);
                        break;
                    }
                }

                var serviceTest = new DashbordExporter();
                serviceTest.Export(thumbnailsPath, dashboardId, dashboardFileExtention, hash);
            }

            int height;

            if (File.Exists(newFile)) //Это костыль. Проблема в функции RestoreXML() в DashboardConfig.cs
            {
                var thumbnail = Image.FromFile(newFile);
                height = thumbnail.Height;
                thumbnail.Dispose();
            }
            else
                height = 0;

            if (height < 100)
            {
                var image = Image.FromFile(HostingEnvironment.MapPath("~/Content/animation/empty.jpg"));

                image = drawOverlay((Bitmap)image, dashboardId);
                image.Save(newFile);
            };


            return hash;
        }

        public void garbageCollect(string thumbnailsPath)
        {
            var storage = DashboardConfigurator.Default.DashboardStorage;
            var dashboardInfo = storage.GetAvailableDashboardsInfo();
            var dirInfo = new DirectoryInfo(thumbnailsPath);

            var count = dashboardInfo.Count();
            var dashboards = dashboardInfo.ToArray();

            foreach (var file in dirInfo.GetFiles())
            {
                bool isGarbage = true;

                var id = file.Name.Substring(0, file.Name.IndexOf('_'));
                for (int index = 0; index < count; index++)
                {
                    if (dashboards[index].ID == id) isGarbage = false;
                }

                if (isGarbage) file.Delete();
            }

        }

        private Bitmap drawOverlay(Bitmap image, string overlayText)
        {
            PointF location = new PointF(10f, 10f);

            using (Graphics graphics = Graphics.FromImage(image))
            {
                using (Font arialFont = new Font("Arial", 80))
                {
                    graphics.DrawString(overlayText, arialFont, Brushes.DarkBlue, location);
                }
            }

            return image;
        }

    }
}