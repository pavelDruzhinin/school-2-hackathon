using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using RosCottedge.Models;
using RosCottedge.ViewModels;

namespace RosCottedge.Controllers
{
    public class HouseController : Controller
    {
        private SiteContext db = new SiteContext();

        //Отображение данных дома
        [HttpGet]
        public ActionResult Index(int houseId)
        {
            var viewModel = new HouseIndexViewModel()
            {
                House = db.Houses.Find(houseId),
                Reviews = db.Reviews.Include(u => u.User).Where(r => r.HouseId == houseId).ToList()
            };

            return View(viewModel);

        }
        
        //Для кнопки "Забронировать" на странице дома.
        [HttpPost]
        public ActionResult AddReservation(Reservation reservation)
        {
            reservation.ReservationDate = DateTime.Now;

            //Создаём список зарезервированных дней в заказе
            var DateRange = new List<DateTime>();
            var arrival = reservation.ArrivalDate;
            var departure = reservation.DepartureDate;
            for (var date = arrival; date <= departure; date = date.AddDays(1)) DateRange.Add(date);

            //Создаём список всех зарезервированных дней из базы
            var AllRange = new List<DateTime>();
            foreach (var x in db.Reservations)
            {
                var _arrival = x.ArrivalDate;
                var _departure = x.DepartureDate;
                for (var _date = _arrival; _date <= _departure; _date = _date.AddDays(1)) AllRange.Add(_date);
            };

            foreach (var y in DateRange)
            {
                if (AllRange.Contains(y))
                {
                    return RedirectToAction("WrongDateRange", "House");
                }
            }

            User user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
            reservation.UserId = user.Id;
            db.Reservations.Add(reservation);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //Страница ошибки, если выбранные даты уже заняты в базе. 
        public ActionResult WrongDateRange()
        {
            return View();
        }
        
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        [HttpPost]
        public ActionResult AddReview(Review review)
        {
            var user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
            var reservation = db.Reservations
                .Where(r => r.UserId == user.Id && r.DepartureDate < DateTime.Now && r.HouseId == review.HouseId)
                .FirstOrDefault();
            if (reservation == null)
            {
                return RedirectToAction("Index", new { houseId = review.HouseId });
            }
            else
            {
                review.CommentDate = DateTime.Now;
                review.UserId = user.Id;
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index", new { houseId = review.HouseId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(House house)
        {
            if (ModelState.IsValid)
            {
                User user
                 = db.Users.Where
                        (x => x.Login == User.Identity.Name).FirstOrDefault();
                house.UserId = user.Id;
                house.Rating = 0;
                db.Houses.Add(house);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(house);
        }
    }
}
