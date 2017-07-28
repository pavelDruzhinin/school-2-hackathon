using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RosCottedge.Controllers
{
    public class GetUserController : Controller
    {
        private SiteContext db = new SiteContext();
        public ActionResult Index()
        {
            ViewBag.User = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
            return PartialView("_User");
        }
    }
}