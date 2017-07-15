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
using PagedList;

namespace RosCottedge.Controllers
{
    public class HouseController : Controller
    {
        private SiteContext db = new SiteContext();

        //Отображение данных дома
        [HttpGet]
        public ActionResult Index(int houseId, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
//            ViewBag.countRev = db.Houses.Select(x => x.Reviews.Count()).FirstOrDefault();

            var viewModel = new HouseIndexViewModel()
            {
                House = db.Houses.Include(u => u.User).FirstOrDefault(x => x.Id == houseId),
                Reviews = db.Reviews.Include(u => u.User).Where(r => r.HouseId == houseId).OrderByDescending(r => r.CommentDate).ToPagedList(pageNumber, pageSize)
            };
            
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

//            var dateLimit = DateTime.Today.AddDays(-14);
//            var reservation = db.Reservations
//                .Where(r => r.UserId == user.Id && r.DepartureDate > dateLimit && r.DepartureDate < DateTime.Now && r.HouseId == review.HouseId)
//                .FirstOrDefault();
//
//            if (reservation == null) //Резерваций, закончившихся менее 14 дней назад не было
//            {
//                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
//            }
//
//            var existingReview = db.Reviews.OrderByDescending(e => e.CommentDate)
//                .Where(e => e.UserId == user.Id && e.HouseId == review.HouseId && e.CommentDate > reservation.DepartureDate)
//                .FirstOrDefault();
//
//            if (existingReview != null) //Юзер уже оставил комментарий
//            {
//                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
//
//            }

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
                db.Houses.Add(house);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(house);
        }
    }
}
