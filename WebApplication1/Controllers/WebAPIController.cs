using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using WebApplication1.Services;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class WebAPIController : Controller
    {
        // POST: WebAPI/Create
        [System.Web.Http.HttpPost]
        public ActionResult Post(string id)
        {
            var dashboard = new Preview();

            if (id != null)
            {
                dashboard.NewDashboardName = id;
                //Dashboard.Add(dashboard.NewDashboardName);
            }

            return new JsonResult()
            {
                Data = id,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [System.Web.Http.HttpGet]
        public ActionResult Get()
        {
                return new JsonResult() { Data = "FullscreenHome",
                JsonRequestBehavior= JsonRequestBehavior.AllowGet};
        }
    }
}
