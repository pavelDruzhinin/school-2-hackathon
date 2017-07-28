﻿using RosCottedge.Models;
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
        
        public ActionResult Index(int? page, string region, int? startPrice, int? finishPrice, int? numberOfPersons, DateTime? arrivalDate, DateTime? departureDate, string sortBy)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;

            var houses = db.Houses.Include(x => x.Reviews).Include(x => x.Reservations).Include(x => x.Pictures).Where(x=>x.Hide==false);

            if (!String.IsNullOrEmpty(region))
            {
                houses = houses.Where(x => x.Region == region || x.Locality == region);
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

            switch (sortBy)
            {
                case "new":
                    houses = houses.OrderByDescending(x => x.Id);
                    break;
                case "rating":
                    houses = houses.OrderBy(x => x.Rating);
                    break;
                case "price":
                    houses = houses.OrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    houses = houses.OrderByDescending(x => x.Price);
                    break;
                default:
                    houses = houses.OrderByDescending(x => x.Id);
                    break;

            }

            //Определяем максимальную и минимальную цену аренды

            int max = 0;
            int min = 0;

            if (db.Houses.Any())
            {
                max = db.Houses.Select(x => x.Price).Max();
                min = db.Houses.Select(x => x.Price).Min();
            }

            ViewBag.MaxPrice = max;
            ViewBag.MinPrice = min;

            //Создаём лист регионов из базы

            List<House> regions = new List<House>();

            foreach (var h in db.Houses)
            {
                if (!regions.Any(x => x.Region == h.Region))
                {
                    regions.Add(h);
                }
            }
            
            var viewModel = new HomeIndexViewModel()
            {
                Houses = houses.ToPagedList(pageNumber, pageSize),
                AllHouses = db.Houses.ToList(),
                Regions = regions.OrderBy(x => x.Region).ToList()
            };
            
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_Houses", viewModel)
                : View(viewModel);
        }

        public ActionResult AutocompleteSearch(string search)
          {
              List<string> RegionsAndLocalities = new List<string>();
              foreach (var r in db.Houses.Where(x => x.Region.Contains(search)))
              {
                  if (!RegionsAndLocalities.Contains(r.Region))
                  {
                      RegionsAndLocalities.Add(r.Region);
                  }
              }
              
              foreach (var r in db.Houses.Where(x => x.Locality.Contains(search)))
              {
                if (!RegionsAndLocalities.Contains(r.Locality))
                {
                    RegionsAndLocalities.Add(r.Locality);
                }
              }

              return Json(RegionsAndLocalities, JsonRequestBehavior.AllowGet);
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
