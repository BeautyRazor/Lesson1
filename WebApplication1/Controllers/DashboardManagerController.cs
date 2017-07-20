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
    public class DashboardManagerController : Controller 

    {
        private readonly ICRUDDashboardStorage storage;

        private class JsonReport
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        public DashboardManagerController()
        {
            storage = (ICRUDDashboardStorage)DashboardConfigurator.Default.DashboardStorage;
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

            dasboards.Dashboards = storage.GetAvailableDashboardsInfo().ToList();

            return View(dasboards);
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


        [System.Web.Http.HttpPost]
        public ActionResult Add(string name)
        {
            var dashboard = new DevExpress.DashboardCommon.Dashboard();

            return RedirectToAction("Partial", new
            {
                id = storage.AddDashboard(dashboard.SaveToXDocument(), name)
            });

        }

        [System.Web.Http.HttpPost]
        public string Delete(string id)
        {
            var data = new JsonReport()
            {
                ID = storage.DeleteDashboard(id),
                Name = "DELETED"
            };

            return new JavaScriptSerializer().Serialize(data);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Clone(string id, string name = "default")
        {
            var data = new JsonReport();

            if(name == "default")
            {
                return RedirectToAction("Partial", new
                {
                    id = storage.CloneDashboard(id)
                });
            }
            else
            {
                return RedirectToAction("Partial", new
                {
                    id = storage.CloneDashboard(id, name)
                });
            }

        }

       

    }
}