﻿using System;
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
using Webdiyer.WebControls.Mvc;

namespace RosCottedge.Controllers
{
    public class HouseController : Controller
    {
        private SiteContext db = new SiteContext();

        //Отображение данных дома
        [HttpGet]
        public ActionResult Index(int houseId, int? page)
        {
            //Проверяем, давать ли пользователю оставить коммент
            var allowComments = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
                var dateLimit = DateTime.Today.AddDays(-14);


                //Ищем закончившиеся менее 14 дней назад брони
                var reservation = db.Reservations
                    .Where(r => r.UserId == user.Id && r.DepartureDate > dateLimit && r.DepartureDate < DateTime.Now && r.HouseId == houseId)
                    .FirstOrDefault();

                //Если брони нашлись, ищем оставленный после завершения
                if (reservation != null)
                {
                    var existingReview = db.Reviews.OrderByDescending(e => e.CommentDate).Where(e => e.UserId == user.Id && e.HouseId == houseId && e.CommentDate > reservation.DepartureDate)
                        .FirstOrDefault();

                    if (existingReview == null)
                    {
                        allowComments = true;
                    }
                }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            var viewModel = new HouseIndexViewModel()
            {
                House = db.Houses.Include(u => u.User).FirstOrDefault(x => x.Id == houseId),
                Reviews = db.Reviews.Include(u => u.User).Where(r => r.HouseId == houseId).OrderByDescending(r => r.CommentDate).ToPagedList(pageNumber, pageSize),
                Pictures = db.Pictures.Where(p => p.HouseId == houseId).ToList(),
                User = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault(),
                AllowComments = true
            };

            var kappa = Request.IsAjaxRequest();
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_Comments", viewModel)
                : View(viewModel);
        }

        //Для кнопки "Забронировать" на странице дома.
        [HttpPost]
        public string AddReservation(int HouseId, DateTime ArrivalDate, DateTime DepartureDate, string Description)
        {

            var reservation = new Reservation { HouseId = HouseId, ArrivalDate = ArrivalDate, DepartureDate = DepartureDate, Description = Description };

            if (User.Identity.IsAuthenticated)
            {
                if (reservation.ArrivalDate < DateTime.Now || reservation.ArrivalDate > reservation.DepartureDate)
                {
                    return "Неверный диапазон дат";
                }
                else
                {
                    reservation.ReservationDate = DateTime.Now;

                    foreach (var r in db.Reservations.Where(h => h.HouseId == HouseId))
                    {
                        if (reservation.ArrivalDate <= r.DepartureDate && r.ArrivalDate <= reservation.DepartureDate)
                        {
                            return "Извините, одна из выбранных вами дат уже забронирована";
                        }
                    }

                    User user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
                    reservation.UserId = user.Id;
                    
                    db.Reservations.Add(reservation);
                    
                    db.SaveChanges();
                    return "<script> document.location.href = '/Home/Index' </script>";
                }
            }
            else
            {
                return "<script> document.location.href = '/Account/Login' </script>";
            }

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

            review.CommentDate = DateTime.Now;
            review.UserId = user.Id;
            
            //Изменяем рейтинг дома в базе.
            var house = db.Houses.Find(review.HouseId);
            var allReviews = db.Reviews.Where(x => x.HouseId == review.HouseId).ToList();
            allReviews.Add(review);
            var reviewsCount = allReviews.Count();
            var ratingSum = allReviews.Sum(x => x.Rating);
            house.Rating = ratingSum / reviewsCount;
            
            db.Reviews.Add(review);
           
            db.SaveChanges();
            
            var viewModel = new HouseIndexViewModel()
            {
                House = db.Houses.Include(u => u.User).FirstOrDefault(x => x.Id == review.HouseId),
                Reviews = db.Reviews.Include(u => u.User).Where(r => r.HouseId == review.HouseId).OrderByDescending(r => r.CommentDate).ToPagedList(1, 5)
            };

            return PartialView("_Comments", viewModel);
            
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
                house.Avatar = "/Content/img/houseAvatar.jpg";
                db.Houses.Add(house);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(house);
        }

        public ActionResult GetDates(int houseId)
        {
            var reservations = db.Reservations.Where(r => r.HouseId == houseId);
            List<string> reservedDates = new List<string>();
            foreach (var reservation in reservations)
            {
                for (var i = reservation.ArrivalDate; i <= reservation.DepartureDate; i=i.AddDays(1))
                {
                    reservedDates.Add(i.ToString("dd-MM-yyyy"));
                }
            }
            var datesArray = reservedDates.ToArray();
            return Json(datesArray, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HouseObject()
        {
            var House = db.Houses.Select(n => new { n.Name, n.Lat, n.Lon });
            return Json(House, JsonRequestBehavior.AllowGet);
        }

    }
}
