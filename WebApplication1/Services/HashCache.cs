using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace WebApplication1.Services
{
    public class HashCache
    {
        public string CacheRequest(string thumbnailsPath, string dashboardPath, string dashboardID, string extension)
        {
            string newFile = dashboardPath + dashboardID + "." + "xml";
            string hash = newFile.GetHashCode().ToString();
            string exFile = HostingEnvironment.MapPath(thumbnailsPath);

            var directory = new DirectoryInfo(exFile).GetFiles();


            newFile = HostingEnvironment.MapPath(thumbnailsPath + dashboardID + "_" + hash + "." + extension);
            
            if (!File.Exists(newFile)) {
                
                foreach (var file in directory)
                {
                    if (Regex.IsMatch(file.Name, dashboardID + "_"))
                    {
                        File.Delete(file.FullName);
                        break;
                    }
                }

                var serviceTest = new StreamTest();
                serviceTest.Export(thumbnailsPath, dashboardID, extension, hash);
            }

            return hash;
        }
    }
}