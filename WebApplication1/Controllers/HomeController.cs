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

        public ActionResult Fullscreen(string dashboardid = "dashboard1")
        {
            var goToFullscreen = new Models.Fullscreen();

            goToFullscreen.currentDashboardId = dashboardid;

            return View(goToFullscreen);
        }

        public ActionResult Preview()
        {
            ViewBag.Message = "Your application description page.";

            var dasboards = new Models.Preview();
            
            string dashboardsPath = @"~\App_Data";
            string thumbnailsPath = @"~\Content\img";

            var serviceTest = new Services.StreamTest();

            DashboardFileStorage storage = new DashboardFileStorage(dashboardsPath);

            serviceTest.Export(storage, thumbnailsPath);

            dasboards.DashboardCount = new DirectoryInfo(HostingEnvironment.MapPath(@"~\App_Data\Dashboards\")).GetFiles().Length;
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
    }
}