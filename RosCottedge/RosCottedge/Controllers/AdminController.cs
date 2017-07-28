using RosCottedge.Models;
using RosCottedge.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Web.Security;
using Webdiyer.WebControls.Mvc;

namespace RosCottedge.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        SiteContext db = new SiteContext();

        public ActionResult AdminPanel()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                User user = (from u in db.Users
                             where u.Login == User.Identity.Name
                             select u).FirstOrDefault();
                return PartialView(user);
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

        public ActionResult ShowAllUsers()
        {
            var user = (from a in db.Users
                        select a).ToList();
            return View(user);
        }
        public ActionResult UserPersonalInformation(int? Id)
        {
            var user = (from u in db.Users
                        where u.Id == Id
                        select u).FirstOrDefault();
            return View(user);

        }
        #region Редактирование пользователя
        public ActionResult AddAvatar(HttpPostedFileBase upload, int? Id)
        {
            User user = (from u in db.Users
                         where u.Id == Id
                         select u).FirstOrDefault();
            if (upload != null)
            {

                string fileName = System.IO.Path.GetFileName(upload.FileName);
                DirectoryInfo Dir = new DirectoryInfo(Request.MapPath("/Content/img/users"));
                Dir.CreateSubdirectory(user.Login);
                upload.SaveAs(Server.MapPath("~/Content/img/users/" + user.Login + "/" + fileName));

                user.Avatar = "/Content/img/users/" + user.Login + "/" + fileName;

                db.SaveChanges();
            }
            return RedirectToAction("UserPersonalInformation", user);
        }
        [HttpGet]
        public ActionResult EditUsers(int? Id)
        {
            User user = (from u in db.Users
                         where u.Id == Id
                         select u).FirstOrDefault();
            return View(user);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsers(User user)
        {
            User olduser = (from u in db.Users
                            where u.Id == user.Id
                            select u).FirstOrDefault();


            if (ModelState.IsValid)
            {

                User email = db.Users.Where(e => e.Email == user.Email).FirstOrDefault();
                if (email == null || olduser.Email == user.Email)
                {
                    user.Avatar = olduser.Avatar;
                    user.OldPassword = user.Password;
                    db.Set<User>().AddOrUpdate(user);
                    db.SaveChanges();
                    return RedirectToAction("ShowAllUsers");

                }
                else { ModelState.AddModelError("Email", "Такой E-mail уже зарегистрирован"); }

            }
            return View(user);

        }
        public ActionResult DeleteUser(int id)
        {

            User user = db.Users.Find(id);
            if (user.Login != User.Identity.Name)
            {
                db.Entry(user)
                   .Collection(c => c.Houses).Load();
                db.Users.Remove(user);
                db.SaveChanges();

            }
            return RedirectToAction("ShowAllUsers");
        }

        [HttpPost]
        public ActionResult DeleteComment(int id, int houseId)
        {
            var review = db.Reviews.Find(id);

            var house = db.Houses.Find(houseId);
            var reviews = db.Reviews.Where(x => x.HouseId == houseId).ToList();
            reviews.Remove(review);

            if (house != null)
            {
                if (reviews.Count == 0)
                {
                    house.Rating = 0;
                }
                else
                {
                    house.Rating = reviews.Sum(x => x.Rating) / reviews.Count();
                }
            }
            db.Reviews.Remove(review);
            
            
            db.SaveChanges();

            return RedirectToAction("Index", "House", new { houseId = houseId });
        }

        #endregion

        #region Дома пользователей

        public ActionResult UsersHouse(int? page)
        {

            int pageNumber = (page ?? 1);
            int pageSize = 8;

            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);


            User user = (from u in db.Users
                         where u.Login == User.Identity.Name
                         select u).FirstOrDefault();

            MyHouseViewModel myHouseModel = new MyHouseViewModel()
            {
                User = user,
                House = (from h in db.Houses
                        .Include(x => x.Pictures)
                         select h).OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize),

            };
            return View(myHouseModel);
        }
        //Удаление дома
        [HttpPost]
        public ActionResult DeleteHouse(int id)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            House house = db.Houses.Find(id);

            db.Houses.Remove(house);
            db.SaveChanges();

            return RedirectToAction("UsersHouse", "Admin");
        }
        #endregion

        #region Редактирование домов
        //Загрузка фотографий дома

        public ActionResult AddPictures(HttpPostedFileBase upload, int houseId)
        {
            if (upload != null)
            {

                House house = db.Houses.Where(y => y.Id == houseId).FirstOrDefault();
                User user = (from u in db.Users
                             where u.Id == house.UserId
                             select u).FirstOrDefault();
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                DirectoryInfo Dir = new DirectoryInfo(Request.MapPath("/Content/img/users"));
                Dir.CreateSubdirectory(user.Login);
                //Проверка на существование на сервере файла с идентичным именем
                FileInfo file = new FileInfo(Server.MapPath("/Content/img/users/" + user.Login + "/" + fileName));
                if (!file.Exists)
                {
                    upload.SaveAs(Server.MapPath("~/Content/img/users/" + user.Login + "/" + fileName));
                    var picture = new Picture
                    {
                        Adress = "/Content/img/users/" + user.Login + "/" + fileName,
                        HouseId = houseId
                    };
                    db.Pictures.Add(picture);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("EditAllHouse", "Admin", new { id = houseId });
        }

        //Удаление изображения
        [HttpPost]
        public ActionResult DeletePicture(int houseId, int pictureId, string path)
        {
            FileInfo file = new FileInfo(Server.MapPath(path));
            if (file.Exists)
            {
                file.Delete();
                var picture = db.Pictures.Where(x => x.Id == pictureId).FirstOrDefault();
                db.Pictures.Remove(picture);
                db.SaveChanges();
            }
            return RedirectToAction("EditAllHouse", "Admin", new { id = houseId });
        }

        public ActionResult EditAllHouse(int? id)
        {

            House house = db.Houses.Find(id);

            User user = (from u in db.Users
                         where u.Login == User.Identity.Name
                         select u).FirstOrDefault();



            EditMyHouseViewModel viewModel = new EditMyHouseViewModel()
            {
                House = house,
                User = user,
                //Вывод всех фотографий дома
                Pictures = db.Pictures.Where(p => p.HouseId == house.Id).ToList(),

                Reservations = (from r in db.Reservations
                       .Include(x => x.User)
                                where r.HouseId == house.Id
                                orderby r.ArrivalDate ascending
                                select r).ToList(),


            };

            return View(viewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveHouse(House house)
        {

            if (ModelState.IsValid)
            {
                var editedHouse = db.Houses.Find(house.Id);

                if (editedHouse != null)
                {
                    db.Entry(editedHouse).CurrentValues.SetValues(house);
                    db.SaveChanges();
                }
                return RedirectToAction("UsersHouse");
            }
            return RedirectToAction("UsersHouse", "Admin", new { id = house.Id });
        }

        #endregion

        #region Все брони
        public ActionResult Trips()
        {

            User user = (from u in db.Users
                         where u.Login == User.Identity.Name
                         select u).FirstOrDefault();

            MyTripsViewModel MyTripsViewModel = new MyTripsViewModel
            {
                User = user,


                ReservationDelete = (from a in db.Reservations
                                     .Include(x => x.House)
                                     .Include(x => x.House.Pictures)

                                     select a).ToList()
            };


            return View(MyTripsViewModel);
        }

        // Отказ от брони
        public ActionResult DeleteReservation(int id, int houseId)
        {

            Reservation reserv = db.Reservations.Find(id);

            ReservDelNotice resDel = new ReservDelNotice
            {
                ArrivalDate = reserv.ArrivalDate,
                DepartureDate = reserv.DepartureDate,
                Description = reserv.Description,
                HouseId = reserv.HouseId,
                Id = reserv.Id,
                ReservationDate = DateTime.Now,
                UserId = reserv.UserId

            };

            db.Reservations.Remove(reserv);
            db.ReservDelNotices.Add(resDel);
            db.SaveChanges();


            return RedirectToAction("Trips", "Admin", new { id = houseId });
        }
        #endregion

    }
}
