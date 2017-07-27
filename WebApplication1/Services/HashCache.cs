
using System.Drawing;
using DevExpress.DashboardWeb;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using DevExpress.DataProcessing.InMemoryDataProcessor.Executors;

namespace WebApplication1.Services
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

            var thumbnail = Image.FromFile(newFile);


            return hash;
        }

        public void garbageCollect(string thumbnailsPath)
        {
            var storage = DashboardConfigurator.Default.DashboardStorage;
            var dashboardInfo = storage.GetAvailableDashboardsInfo();
            var dirInfo = new DirectoryInfo(thumbnailsPath);

            var count = dashboardInfo.Count();
            var dashboards = dashboardInfo.ToArray();

            foreach (var file in dirInfo.GetFiles() )
            {
                bool isGarbage = true;

                var id = file.Name.Substring(0, file.Name.IndexOf('_'));
                for (int index = 0; index < count; index++)
                {
                    if (dashboards[index].ID == id) isGarbage = false;
                }

                if(isGarbage) file.Delete();
            }

        }
    }
}