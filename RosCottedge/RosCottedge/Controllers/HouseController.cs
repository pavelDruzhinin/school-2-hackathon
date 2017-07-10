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
            User user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
            reservation.UserId = user.Id;
            db.Reservations.Add(reservation);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", new {houseId = review.HouseId});
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
                return RedirectToAction("Index","Home");
            }
            
            return View(house);
        }
    }
}
