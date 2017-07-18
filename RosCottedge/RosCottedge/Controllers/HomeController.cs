using PagedList;
using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace RosCottedge.Controllers
{
    public class HomeController : Controller
    {
        private SiteContext db = new SiteContext();

        public ActionResult Index(int? page, string region, int? startPrice, int? finishPrice, int? numberOfPersons, DateTime? arrivalDate, DateTime? departureDate)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 8;

            IEnumerable<House> houses = db.Houses.Include(x => x.Reviews).Include(x => x.Reservations);
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
            if (arrivalDate != null && departureDate != null)
            {
                houses = houses.Where(x => !x.Reservations.Any(r => r.ArrivalDate <= departureDate && arrivalDate <= r.DepartureDate));
            }

            //Определяем максимальную и минимальную цену аренды

            int max = db.Houses.Select(x=> x.Price).Max();
            int min = db.Houses.Select(x => x.Price).Min();

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