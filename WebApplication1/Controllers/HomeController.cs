using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Viewer(string id)
        {
            var goToFullscreen = new Fullscreen()
            {
                currentDashboardId = id
            };

            return View(goToFullscreen);
        }

        public ActionResult Designer(string id)
        {
            var goToFullscreen = new Fullscreen()
            {
                currentDashboardId = id
            };

            return View(goToFullscreen);
        }

        public ActionResult Preview()
        {
            ViewBag.Message = "Your application description page.";

            var dasboards = new Preview();
            
            string dashboardsPath = @"~\App_Data\Dashboards";

            DashboardFileStorage storage = new DashboardFileStorage(dashboardsPath);
            
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

        public ActionResult Thumbnail(string id)
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

        public ActionResult Partial(string id)
        {
            var getPartial = new Partial();
            getPartial.DashboardID = id;

            return View(getPartial);
        }

        public class JsonReport
        {
            public string ID { get; set; }
            public string title { get; set; }
        }

        [System.Web.Http.HttpPost]
        public string Add(string name)
        {
            var dashboard = new Dashboard();
            var data = new JsonReport()
            {
                ID = dashboard.Add(name),
                title = name
            };

            return new JavaScriptSerializer().Serialize(data);
        }

        [System.Web.Http.HttpPost]
        public string Delete(string id)
        {
            var dashboard = new Dashboard(id);
            dashboard.Delete();
            var data = new JsonReport()
            {
                ID = id,
                title = "DELETED"
            };

            return new JavaScriptSerializer().Serialize(data);
        }

        [System.Web.Http.HttpPost]
        public string Clone(string id, string name = "default")
        {
            var dashboard = new Dashboard(id);
            var data = new JsonReport();

            if (name == "default")
            {
                data.ID = dashboard.Clone();
            }
            else
            {
                data.ID = dashboard.Clone(name);
                data.title = name;
            }

            return new JavaScriptSerializer().Serialize(data);
        }
    }
}