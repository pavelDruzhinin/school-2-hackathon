using PagedList;
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

        public ActionResult Index(int? page, string region, int? startPrice, int? finishPrice, int? numberOfPersons)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 8;

            IQueryable<House> houses = db.Houses;
            if (!String.IsNullOrEmpty(region))
            {
                houses = houses.Where(x => x.Region == region);
            }
            if (startPrice != null)
            {
                houses = houses.Where(x => x.Price >= startPrice);
            }
            if (finishPrice != null)
            {
                houses = houses.Where(x => x.Price <= finishPrice);
            }
            if (numberOfPersons != null)
            {
                houses = houses.Where(x => x.NumberOfPersons >= numberOfPersons);
            }

            //Определяем максимальную и минимальную цену аренды
            var Price = db.Houses.Select(x => x.Price);
            int max = 0;
            int min = 999;
            foreach(var price in Price)
            {
                if (max < price) max = price;
                if (min > price) min = price;
            }
            ViewBag.MaxPrice = max;
            ViewBag.MinPrice = min;

            return View(houses.OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize));
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