﻿using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Fullscreen(string dashboardid = "default", string dashboardMode = "designer", string isNewDashboard = "false")
        {
            var goToFullscreen = new Models.Fullscreen()
            {
                currentDashboardId = dashboardid
            };
            
            if(isNewDashboard == "true")
            {
                goToFullscreen.currentDashboardId = Services.Dashboard.Add();
            }

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

            var dasboards = new Models.Preview();
            
            string dashboardsPath = @"~\App_Data\Dashboards";
            //string thumbnailsPath = @"~\Content\img";

            //var serviceTest = new Services.StreamTest();

            DashboardFileStorage storage = new DashboardFileStorage(dashboardsPath);

            //serviceTest.Export(storage, thumbnailsPath);

            dasboards.DashboardCount = new DirectoryInfo(HostingEnvironment.MapPath(dashboardsPath)).GetFiles().Length;
            dasboards.Dashboards = (storage as IDashboardStorage).GetAvailableDashboardsInfo().ToList();

            return View(dasboards);
        }

        private object DirectoryInfo(string v)
        {
            throw new NotImplementedException();
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

        public ActionResult Image(string id)
        {
            string thumbnailsPath = @"~\Content\img\";
            string dashboardPath = @"~\App_Data\Dashboards\";

            var extension = "png";

            var cache = new Services.HashCache();

            var hash = cache.CacheRequest(thumbnailsPath, dashboardPath, id, extension);

            var dir = Server.MapPath("/Content/img");
            var path = Path.Combine(dir, id + "_" + hash + "." + extension); //validate the path for security or use other means to generate the path.

            return base.File(path, "image/" + extension);
        }
    }
}