using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Webdiyer.WebControls.Mvc;
using RosCottedge.ViewModels;

namespace RosCottedge.Controllers
{
    public class HomeController : Controller
    {
        private SiteContext db = new SiteContext();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(int? page, string region, int? startPrice, int? finishPrice, int? numberOfPersons, DateTime? arrivalDate, DateTime? departureDate, int? fromForm)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 8;

            var houses = db.Houses.Include(x => x.Reviews).Include(x => x.Reservations).Include(x => x.Pictures);

            if (Session["Filter"] != null && fromForm != 1)
            {
                var oldFilter = (HomeFilter)Session["Filter"];
                region = region ?? oldFilter.Region;
                startPrice = startPrice ?? oldFilter.StartPrice;
                finishPrice = finishPrice ?? oldFilter.FinishPrice;
                numberOfPersons = numberOfPersons ?? oldFilter.NumberOfPersons;
                arrivalDate = arrivalDate ?? oldFilter.ArrivalDate;
                departureDate = departureDate ?? oldFilter.DepartureDate;
            }

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

            var filter = new HomeFilter
            {
                Region = region,
                StartPrice = startPrice,
                FinishPrice = finishPrice,
                NumberOfPersons = numberOfPersons,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                Page = pageNumber
            };

            Session["Filter"] = filter;

            //Определяем максимальную и минимальную цену аренды

            int max = 0;
            int min = 0;

            if (db.Houses.Any())
            {
                max = db.Houses.Select(x => x.Price).Max();
                min = db.Houses.Select(x => x.Price).Min();
            };

            ViewBag.MaxPrice = max;
            ViewBag.MinPrice = min;

            //Создаём лист регионов из базы

            List<string> regions = new List<string>();
            
            foreach (var h in db.Houses)
            {
                if (!regions.Contains(h.Region))
                {
                    regions.Add(h.Region);
                }
            };

            var viewModel = new HomeIndexViewModel()
            {
                Houses = houses.OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize),
                Regions = regions
            };
            
        var kappa = Request.IsAjaxRequest();

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_Houses", viewModel)
                : View(viewModel);
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