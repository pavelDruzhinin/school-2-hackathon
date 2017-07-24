using RosCottedge.Models;
using RosCottedge.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
                    user.RegistrationDate = DateTime.Now;
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


            User olduser = (from u in db.Users
                            where u.Login == User.Identity.Name
                            select u).FirstOrDefault();


            if (ModelState.IsValid)
            {

                User email = db.Users.Where(e => e.Email == user.Email).FirstOrDefault();
                if (email == null || olduser.Email == user.Email)
                {

                    db.Set<User>().AddOrUpdate(user);
                    db.SaveChanges();
                    return RedirectToAction("PersonalInformation");

                }
                else { ModelState.AddModelError("Email", "Такой E-mail уже зарегистрирован"); }

            }
            return View(user);

        }
        [HttpGet]
        public ActionResult ChangePassword(int id)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            User user = db.Users.Find(id);

            return View(user);

        }
        [HttpPost]
        public ActionResult ChangePassword(User user)
        {
            if (ModelState.IsValid)
            {
                User olduser = (from u in db.Users
                                where u.Login == User.Identity.Name
                                select u).FirstOrDefault();
                if (olduser.OldPassword == user.OldPassword)
                {
                    user.OldPassword = user.Password;
                    db.Set<User>().AddOrUpdate(user);
                    db.SaveChanges();
                    return RedirectToAction("PersonalInformation");
                }
                else { ModelState.AddModelError("OldPassword", "Неверный старый пароль"); }
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

            List<GeneralСlass> genclass = new List<GeneralСlass>();

            //Ищем дома, по которым пришла бронь
            IQueryable<Reservation> reservations = from r in db.Reservations
                                .Include(u => u.User).Include(u=>u.House)
                               where r.House.UserId == user.Id && r.Landlord == false
                               select r;
            //Ищем дома, по которым оставлен отзыв
            IQueryable<Review> reviews = from c in db.Reviews
                     .Include(x => x.User).Include(x => x.House)
                          where c.House.UserId == user.Id && c.Landlord == false
                          select c;
            //Ищем дома по которым пришла отмена брони
            IQueryable<ReservDelNotice> reservdelnotices = from c in db.ReservDelNotices
                             .Include(x => x.User).Include(x => x.House)
                                                           where c.House.UserId == user.Id
                                                           select c;

            foreach (var r in reservations)
            {
                var instanceGeneralClass = new GeneralСlass();
                #region
                instanceGeneralClass.Date = r.ReservationDate;
                instanceGeneralClass.ArrivalDate = r.ArrivalDate;
                instanceGeneralClass.DepartureDate = r.DepartureDate;
                instanceGeneralClass.Description = r.Description;
                instanceGeneralClass.Tenant = r.Tenant;
                instanceGeneralClass.Landlord = r.Landlord;
                instanceGeneralClass.UserId = r.UserId;
                instanceGeneralClass.HouseId = r.HouseId;
                instanceGeneralClass.User = r.User;
                instanceGeneralClass.House = r.House;
                instanceGeneralClass.Reserv = true;
                instanceGeneralClass.Id = r.Id;
                #endregion
                genclass.Add(instanceGeneralClass);
            }
            foreach (var r in reviews)
            {
                var instanceGeneralClass = new GeneralСlass();
                #region
                instanceGeneralClass.Comment = r.Comment;
                instanceGeneralClass.Rating = r.Rating;
                instanceGeneralClass.Date = r.CommentDate;
                instanceGeneralClass.UserId = r.UserId;
                instanceGeneralClass.HouseId = r.HouseId;
                instanceGeneralClass.Landlord = r.Landlord;
                instanceGeneralClass.User = r.User;
                instanceGeneralClass.House = r.House;
                instanceGeneralClass.Landlord = r.Landlord;
                instanceGeneralClass.Id = r.Id;
                instanceGeneralClass.Review = true;
                #endregion
                genclass.Add(instanceGeneralClass);
            }

            foreach (var r in reservdelnotices)
            {
                #region
                var instanceGeneralClass = new GeneralСlass();
                instanceGeneralClass.Date = r.ReservationDate;
                instanceGeneralClass.ArrivalDate = r.ArrivalDate;
                instanceGeneralClass.DepartureDate = r.DepartureDate;
                instanceGeneralClass.Description = r.Description;
                instanceGeneralClass.UserId = r.UserId;
                instanceGeneralClass.HouseId = r.HouseId;
                instanceGeneralClass.User = r.User;
                instanceGeneralClass.House = r.House;                          
                instanceGeneralClass.Id = r.Id;
                #endregion
                genclass.Add(instanceGeneralClass);
            }

            // Выводим все дома, которые добавлял пользователь
            MyHouseViewModel myHouseModel = new MyHouseViewModel()
            {
                House = from h in db.Houses
                        .Include(x => x.Pictures)
                        where h.UserId == user.Id && h.Hide==false
                        select h,
                
                GeneralClass= from g in genclass
                              orderby g.Date descending
                              select g

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

            
               var ho = (from h in db.Reservations
                        where h.House.User.Login == User.Identity.Name && h.HouseId == house.Id
                        orderby h.DepartureDate
                      select h).ToList().LastOrDefault();

            if (house.Reservations == null || ho.DepartureDate<DateTime.Now)
            {
                house.Hide = true;
                db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("","По этому дому есть бронь. Удаление невозможно");
            }
           
            return RedirectToAction("MyHouse", "Account");
        }
        #endregion

        #region Редактирование моих домов

        //Загрузка фотографий дома

        public ActionResult AddPictures(HttpPostedFileBase upload, int houseId)
        {
            if (upload != null)
            {
                User user = db.Users.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
                House house = db.Houses.Where(y => y.Id == houseId).FirstOrDefault();
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


                List<GeneralСlass> genclass = new List<GeneralСlass>();
                if (house.UserId != user.Id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                //Вывод забронированных дат по выбранному дому
                IQueryable<Reservation> reservations = from r in db.Reservations
                                            .Include(x => x.User)
                                                       where r.House.UserId == user.Id && r.HouseId == house.Id && r.Landlord == false
                                                       select r;

                //Вывод комментариев по выбранному дому
                IQueryable<Review> reviews = from c in db.Reviews
                                         .Include(x => x.User).Include(x => x.House)
                                             where c.House.UserId == user.Id && c.HouseId == house.Id && c.Landlord == false
                                             select c;

                //Вывод удалённых броней по выбранному дому
                IQueryable<ReservDelNotice> reservdelnotices = from r in db.ReservDelNotices
                                            .Include(x => x.User)
                                   where r.House.UserId == user.Id && r.HouseId == house.Id
                                   select r;

                foreach (var r in reservations)
                {
                    var instanceGeneralClass = new GeneralСlass();
                    #region
                    instanceGeneralClass.Date = r.ReservationDate;
                    instanceGeneralClass.ArrivalDate = r.ArrivalDate;
                    instanceGeneralClass.DepartureDate = r.DepartureDate;
                    instanceGeneralClass.Description = r.Description;
                    instanceGeneralClass.Tenant = r.Tenant;
                    instanceGeneralClass.Landlord = r.Landlord;
                    instanceGeneralClass.UserId = r.UserId;
                    instanceGeneralClass.HouseId = r.HouseId;
                    instanceGeneralClass.User = r.User;
                    instanceGeneralClass.House = r.House;                   
                    instanceGeneralClass.Reserv = true;
                    instanceGeneralClass.Id = r.Id;
                    #endregion
                    genclass.Add(instanceGeneralClass);
                }
                foreach (var r in reviews)
                {
                    var instanceGeneralClass = new GeneralСlass();
                    #region
                    instanceGeneralClass.Comment = r.Comment;
                    instanceGeneralClass.Rating = r.Rating;
                    instanceGeneralClass.Date = r.CommentDate;
                    instanceGeneralClass.UserId = r.UserId;
                    instanceGeneralClass.HouseId = r.HouseId;
                    instanceGeneralClass.Landlord = r.Landlord;
                    instanceGeneralClass.User = r.User;
                    instanceGeneralClass.House = r.House;
                    instanceGeneralClass.Landlord = r.Landlord;
                    instanceGeneralClass.Id = r.Id;
                    instanceGeneralClass.Review = true;
                    
                    #endregion
                    genclass.Add(instanceGeneralClass);
                }

                foreach (var r in reservdelnotices)
                {
                    #region
                    var instanceGeneralClass = new GeneralСlass();
                    instanceGeneralClass.Date = r.ReservationDate;
                    instanceGeneralClass.ArrivalDate = r.ArrivalDate;
                    instanceGeneralClass.DepartureDate = r.DepartureDate;
                    instanceGeneralClass.Description = r.Description;
                    instanceGeneralClass.UserId = r.UserId;
                    instanceGeneralClass.HouseId = r.HouseId;
                    instanceGeneralClass.User = r.User;
                    instanceGeneralClass.House = r.House;                                             
                    instanceGeneralClass.Id = r.Id;
                    #endregion
                    genclass.Add(instanceGeneralClass);
                }
                EditMyHouseViewModel viewModel = new EditMyHouseViewModel()
                {
                    House = house,

                    //Вывод всех фотографий дома
                    Pictures = db.Pictures.Where(p => p.HouseId == house.Id),

                    
                    GeneralClass = from g in genclass
                                   orderby g.Date descending
                                   select g

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
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            Reservation reserv = db.Reservations.Find(id);
            reserv.Landlord = true;
            db.SaveChanges();
            return RedirectToAction("MyHouse", "Account", new { id = houseId });
        }
        //Удаление отзывов из ЛК
        [HttpPost]
        public ActionResult DeleteReviews(int id, int houseId)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            Review review = db.Reviews.Find(id);
            review.Landlord = true;
            db.SaveChanges();
            return RedirectToAction("MyHouse", "Account", new { id = houseId });
        }
        //Удаление оповещения об отмене брони
        [HttpPost]
        public ActionResult DeleteReservationNotif(int id, int houseId)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            ReservDelNotice reserv = db.ReservDelNotices.Find(id);
            db.ReservDelNotices.Remove(reserv);
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

            MyTripsViewModel MyTripsViewModel = new MyTripsViewModel
            {
             ReservationHistory = from a in db.Reservations
                         .Include(x => x.House)
                         where a.UserId == user.Id && a.Tenant == false && a.DepartureDate<DateTime.Now
                         select a,

            ReservationDelete = from a in db.Reservations
                                 .Include(x => x.House)
                                 .Include(x => x.House.Pictures)
                                where a.UserId == user.Id
                                select a
            };
            

            return View(MyTripsViewModel);
        }
        // Удаление из истории моих поездок
        public ActionResult DeleteReservationTenant(int id, int houseId)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            Reservation reserv = db.Reservations.Find(id);
            reserv.Tenant = true;
            db.SaveChanges();
            return RedirectToAction("MyTrips", "Account", new { id = houseId });
        }
        // Отказ от брони
        public ActionResult DeleteReservation(int id, int houseId)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

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


            return RedirectToAction("MyTrips", "Account", new { id = houseId });
        }
        #endregion

        #region Валидация при регистрации
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

        public JsonResult IsPassAvailable(string OldPassword)
        {
            User olduser = (from u in db.Users
                            where u.Login == User.Identity.Name
                            select u).FirstOrDefault();
            if (olduser.OldPassword == OldPassword)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}