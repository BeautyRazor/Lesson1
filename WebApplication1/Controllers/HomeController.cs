using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Fullscreen()
        {
            return View();
        }

        public ActionResult Preview()
        {
            ViewBag.Message = "Your application description page.";

            return View();
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