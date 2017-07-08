using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RosCottedge.Controllers
{
    public class HomeController : Controller
    {
        private SiteContext db = new SiteContext();
        
        public ActionResult Index()
        {
            var house = db.Houses;
            return View(house);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}