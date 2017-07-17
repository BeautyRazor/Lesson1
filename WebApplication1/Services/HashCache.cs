using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace WebApplication1.Services
{
    public class HashCache
    {
        public string CacheRequest(string thumbnailsPath, string dashboardPath, string dashboardID, string dashboardFileExtention)
        {
            string newFile = dashboardPath + dashboardID + "." + "xml";

            var sha256 = SHA256.Create().ComputeHash(File.ReadAllBytes(HostingEnvironment.MapPath(newFile)));
            string hash = "";

            foreach (var h in sha256)
                hash += h.ToString("x2");

            string exFile = HostingEnvironment.MapPath(thumbnailsPath);

            newFile = HostingEnvironment.MapPath(thumbnailsPath + dashboardID + "_" + hash + "." + dashboardFileExtention);
            
            if (!File.Exists(newFile)) {
                var directory = new DirectoryInfo(exFile).GetFiles();

                foreach (var file in directory)
                {
                    if (Regex.IsMatch(file.Name, dashboardID + "_"))
                    {
                        File.Delete(file.FullName);
                        break;
                    }
                }

                var serviceTest = new StreamTest();
                serviceTest.Export(thumbnailsPath, dashboardID, dashboardFileExtention, hash);
            }

            return hash;
        }
    }
}