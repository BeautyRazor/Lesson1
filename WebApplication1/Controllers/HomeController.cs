using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
           
        public ActionResult Fullscreen(string dashboardidt = "default", string dashboardMode = "designer")
        {
            var goToFullscreen = new Fullscreen()
            {
                currentDashboardId = dashboardidt
            };

            switch (dashboardMode)
            {
                case "designer":
                    goToFullscreen.currentDashboardMode = WorkingMode.Designer;
                    break;
                case "viewer":
                    goToFullscreen.currentDashboardMode = WorkingMode.ViewerOnly;
                    break;

                default:
                    break;
            }

            return View(goToFullscreen);
        }

        public ActionResult Preview()
        {
            ViewBag.Message = "Your application description page.";

            var dasboards = new Preview();
            
            string dashboardsPath = @"~\App_Data\Dashboards";
            //string thumbnailsPath = @"~\Content\img";

            //var serviceTest = new Services.StreamTest();

            DashboardFileStorage storage = new DashboardFileStorage(dashboardsPath);

            //serviceTest.Export(storage, thumbnailsPath);
            
            dasboards.DashboardCount = new DirectoryInfo(HostingEnvironment.MapPath(dashboardsPath)).GetFiles().Length;
            dasboards.Dashboards = (storage as IDashboardStorage).GetAvailableDashboardsInfo().ToList();

            return View(dasboards);
        }
        

        public ActionResult Manager()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Manual()
        {
            ViewBag.Message = "Dashboard";

            return View();
        }

        public ActionResult Image(string dashboardidt)
        {
            string thumbnailsPath = @"~\Content\img\";
            string dashboardPath = @"~\App_Data\Dashboards\";

            var extension = "png";

            var cache = new Services.HashCache();

            var hash = cache.CacheRequest(thumbnailsPath, dashboardPath, dashboardidt, extension);

            var dir = Server.MapPath("/Content/img");
            var path = Path.Combine(dir, dashboardidt + "_" + hash + "." + extension); //validate the path for security or use other means to generate the path.

            return base.File(path, "image/" + extension);
        }
    }
}