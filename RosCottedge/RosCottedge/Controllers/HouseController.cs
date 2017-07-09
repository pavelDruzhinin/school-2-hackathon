using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RosCottedge.Models;

namespace RosCottedge.Controllers
{
    public class HouseController : Controller
    {
        private SiteContext db = new SiteContext();

        //Отображение данных дома
        [HttpGet]
        public ActionResult Index(int houseId)
        {
            House house = db.Houses.Find(houseId);
            return View(house);
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(House house)
        {
            if (ModelState.IsValid)
            {
                User user
                 = db.Users.Where
                        (x => x.Login == User.Identity.Name).FirstOrDefault();
                house.UserId = user.Id;
                db.Houses.Add(house);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            
            return View(house);
        }
    }
}
