using RosCottedge.Models;
using RosCottedge.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RosCottedge.Controllers
{
    public class AccountController : Controller
    {
        SiteContext db = new SiteContext();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        #region Логин
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Profile");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)//если модель проходит валидацию, то ищем в базе соответствие 
            {
                var existingUser = db.Users.Where(x => x.Login == user.Login && x.Password == user.Password).FirstOrDefault();

                if (existingUser != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Login, true);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
            }
            return View(user);
        }
        #endregion

        #region Регистрация
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return new HttpStatusCodeResult(HttpStatusCode.Found);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (User.Identity.IsAuthenticated)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (ModelState.IsValid)//если модель проходит валидацию, то в базе ищем логин
            {
                var existingUser = db.Users.Where(x => x.Login == user.Login).FirstOrDefault();
                if (existingUser == null)//если такого пользователя нет, то добавляем в базу нового user
                {
                    user.Avatar = "/Content/img/zlad.jpg";
                    db.Users.Add(user);
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(user.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Такой пользователь уже существует");
                }

            }
            return View(user);
        }
        #endregion

        #region Выход
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        #endregion

        //Личный кабинет

        #region Личная информация

        //Загрузка аватара

        public ActionResult AddAvatar(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                User user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                DirectoryInfo Dir = new DirectoryInfo(Request.MapPath("/Content/img/users"));
                Dir.CreateSubdirectory(user.Login);
                upload.SaveAs(Server.MapPath("~/Content/img/users/" + user.Login + "/" + fileName));

                user.Avatar = "/Content/img/users/" + user.Login + "/" + fileName;

                db.SaveChanges();
            }
            return RedirectToAction("PersonalInformation");
        }

        public new ActionResult Profile()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                User user = (from u in db.Users
                             where u.Login == User.Identity.Name
                             select u).FirstOrDefault();
                return View(user);
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }
        public ActionResult PersonalInformation()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
                return View(user);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

        #endregion

        #region Редактирование личной информации
        [HttpGet]
        public ActionResult Edit()
        {

            if (User.Identity.IsAuthenticated)
            {
                User user = (from u in db.Users
                             where u.Login == User.Identity.Name
                             select u).FirstOrDefault();
                return View(user);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(user);


        }
        #endregion

        #region Мои дома
        public ActionResult MyHouse()
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);


            User user = (from u in db.Users
                         where u.Login == User.Identity.Name
                         select u).FirstOrDefault();

            // Выводим все дома, которые добавлял пользователь
            MyHouseViewModel myHouseModel = new MyHouseViewModel()
            {
                House = from h in db.Houses
                        where h.UserId == user.Id
                        select h,

                //Выводим дома, по которым пришла бронь
                Reservations = from r in db.Reservations
                                .Include(u => u.User)
                              where r.House.UserId == user.Id && r.Landlord==false
                              select r,

                //Выводим дома, по которым оставлен отзыв 
                Reviews = from c in db.Reviews
                         .Include(x => x.User).Include(x => x.House)
                         where c.House.UserId == user.Id && c.Landlord ==false
                         select c

            };

            return View(myHouseModel);
        }
        #endregion

        #region Редактирование моих домов

        //Загрузка фотографий дома

        public ActionResult AddPictures(HttpPostedFileBase upload, int houseId)
        {
            if (upload != null)
            {
                User user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                DirectoryInfo Dir = new DirectoryInfo(Request.MapPath("/Content/img/users"));
                Dir.CreateSubdirectory(user.Login);
                upload.SaveAs(Server.MapPath("~/Content/img/users/" + user.Login + "/" + fileName));

                var picture = new Picture
                {
                    Adress = "/Content/img/users/" + user.Login + "/" + fileName,
                    HouseId = houseId
                };

                db.Pictures.Add(picture);
                db.SaveChanges();
            }
            return RedirectToAction("EditMyHouse", "Account", new { id = houseId });
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
            return RedirectToAction("EditMyHouse", "Account", new { id = houseId });
        }

        public ActionResult EditMyHouse(int? id)
        {

            if (User.Identity.IsAuthenticated)
            {
                House house = db.Houses.Find(id);

                User user = (from u in db.Users
                             where u.Login == User.Identity.Name
                             select u).FirstOrDefault();
                if (house.UserId != user.Id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                EditMyHouseViewModel viewModel = new EditMyHouseViewModel()
                {
                    House = house,

                    //Вывод всех фотографий дома
                    Pictures = db.Pictures.Where(p => p.HouseId == house.Id),

                    //Вывод забронированных дат по выбранному дому
                    Reservations = from r in db.Reservations
                                                .Include(x => x.User)
                                   where r.House.UserId == user.Id && r.HouseId == house.Id && r.Landlord==false
                                   select r,
                    //Вывод комментариев по выбранному дому
                    Reviews = from c in db.Reviews
                                             .Include(x => x.User).Include(x => x.House)
                              where c.House.UserId == user.Id && c.HouseId == house.Id && c.Landlord==false
                              select c

                };

                return View(viewModel);

            }
            else{return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);}
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveHouse(House house)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (ModelState.IsValid)
            {
                var editedHouse = db.Houses.Find(house.Id);

                if (editedHouse != null)
                {
                    db.Entry(editedHouse).CurrentValues.SetValues(house);
                    db.SaveChanges();
                }
                return RedirectToAction("MyHouse");
            }
            return RedirectToAction("EditMyHouse", "Account", new { id = house.Id });
        }
        //Удаление броней из ЛК
        [HttpPost]
        public ActionResult DeleteReservationLandlord(int id, int houseId)
        {
            Reservation reserv = db.Reservations.Find(id);
            reserv.Landlord = true;
            db.SaveChanges();
            return RedirectToAction("MyHouse", "Account", new { id = houseId });
        }
        //Удаление отзывов из ЛК
        [HttpPost]
        public ActionResult DeleteReviews(int id, int houseId)
        {
            Review review = db.Reviews.Find(id);
            review.Landlord = true;
            db.SaveChanges();
            return RedirectToAction("MyHouse", "Account", new { id = houseId });
        }
        #endregion

        #region Мои поездки
        public ActionResult MyTrips()
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            User user = (from u in db.Users
                         where u.Login == User.Identity.Name
                         select u).FirstOrDefault();

            var reserv = from a in db.Reservations
                         .Include(x => x.House)
                         where a.UserId == user.Id && a.Tenant == false
                         select a;


            return View(reserv);
        }
        // Удаление из Мои поездки
        public ActionResult DeleteReservationTenant(int id, int houseId)
        {
            Reservation reserv = db.Reservations.Find(id);
            reserv.Tenant = true;
            db.SaveChanges();
            return RedirectToAction("MyTrips", "Account", new { id = houseId });
        }
        #endregion

        public JsonResult IsLoginAvailable(string Login)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == Login);
            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsPhoneAvailable(string Phone)
        {
            User user = db.Users.FirstOrDefault(u => u.Phone == Phone);
            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailAvailable(string Email)
        {
            User user = db.Users.FirstOrDefault(u => u.Email == Email);
            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}