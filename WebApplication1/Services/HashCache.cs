using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace WebApplication1.Services
{
    public class HashCache
    {
        public string CacheRequest(string thumbnailsPath, string dashboardPath, string dashboardId, string dashboardFileExtention)
        {
            var newFile = dashboardPath + dashboardId + "." + "xml";

            var sha256 = SHA256.Create().ComputeHash(File.ReadAllBytes(HostingEnvironment.MapPath(newFile)));
            var hash = "";

            foreach (var h in sha256)
                hash += h.ToString("x2");

            var exFile = HostingEnvironment.MapPath(thumbnailsPath);

            newFile = HostingEnvironment.MapPath(thumbnailsPath + dashboardId + "_" + hash + "." + dashboardFileExtention);
            
            if (!File.Exists(newFile)) {
                var directory = new DirectoryInfo(exFile).GetFiles();

                foreach (var file in directory)
                {
                    if (Regex.IsMatch(file.Name, dashboardId + "_"))
                    {
                        File.Delete(file.FullName);
                        break;
                    }
                }

                var serviceTest = new DashbordExporter();
                serviceTest.Export(thumbnailsPath, dashboardId, dashboardFileExtention, hash);
            }

            return hash;
        }
    }
}