using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class DashboardManagerController : Controller 

    {
        private readonly ICrudDashboardStorage _storage;

        private class JsonReport
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public DashboardManagerController()
        {
            _storage = (ICrudDashboardStorage)DashboardConfigurator.Default.DashboardStorage;
        }


        public ActionResult Viewer(string id)
        {
            var goToFullscreen = new Fullscreen
            {
                CurrentDashboardId = id
            };

            return View(goToFullscreen);
        }

        public ActionResult Designer(string id)
        {
            var goToFullscreen = new Fullscreen
            {
                CurrentDashboardId = id
            };

            return View(goToFullscreen);
        }
        [Authorize]
        public ActionResult Preview()
        {
            ViewBag.Message = "Your application description page.";

            var thumbnailsPath = Server.MapPath("/Content/img/");
            var cache = new HashCache();
            cache.garbageCollect(thumbnailsPath);

            var dasboards = new Preview {Dashboards = _storage.GetAvailableDashboardsInfo().ToList()};


            return View(dasboards);
        }
        
        public ActionResult Thumbnail(string id)
        {
            var thumbnailsPath = Server.MapPath("/Content/img/");
            var dashboardPath = Server.MapPath("/App_Data/Dashboards/");

            var extension = "png";

            var cache = new HashCache();

            var hash = cache.CacheRequest(thumbnailsPath, dashboardPath, id, extension);

            var dir = Server.MapPath("/Content/img");
            var path = Path.Combine(dir, id + "_" + hash + "." + extension); //validate the path for security or use other means to generate the path.

            return File(path, "image/" + extension);
        }

        public ActionResult Partial(string id)
        {
            var getPartial = new Partial {DashboardId = id};

            return View(getPartial);
        }


        [System.Web.Http.HttpPost]
        public ActionResult Add(string name)
        {
            var dashboard = new Dashboard();

            return RedirectToAction("Partial", new
            {
                id = _storage.AddDashboard(dashboard.SaveToXDocument(), name)
            });

        }

        [System.Web.Http.HttpPost]
        public string Delete(string id)
        {
            var data = new JsonReport
            {
                Id = _storage.DeleteDashboard(id),
                Name = "DELETED"
            };

            return new JavaScriptSerializer().Serialize(data);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Clone(string id, string name = "default")
        {
            if(name == "default")
            {
                return RedirectToAction("Partial", new
                {
                    id = _storage.CloneDashboard(id)
                });
            }
            return RedirectToAction("Partial", new
            {
                id = _storage.CloneDashboard(id, name)
            });
        }

       

    }
}