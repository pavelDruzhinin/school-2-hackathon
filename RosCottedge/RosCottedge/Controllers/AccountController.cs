using RosCottedge.Models;
using RosCottedge.Models.Login_register;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        User user = null;
        // GET: Accaunt
        public ActionResult Index()
        {
            return View();
        }
        #region Логин
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated == true)
                return new HttpStatusCodeResult(HttpStatusCode.Found);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login loginModel)
        {
            if (ModelState.IsValid)//если модель проходит валидацию, то ищем в базе соответствие 
            {
                user = db.Users.FirstOrDefault
                     (x => x.Login == loginModel.LoginName && x.Password == loginModel.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(loginModel.LoginName, true);

                    return RedirectToAction("Index", "Home");
                }

                else { ModelState.AddModelError("", "Неверный логин или пароль"); }
            }
            return View(loginModel);
        }
        #endregion

        #region Регистрация
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated == true)
                return new HttpStatusCodeResult(HttpStatusCode.Found);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register registerModel)
        {
            if (User.Identity.IsAuthenticated == true)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (ModelState.IsValid)//если модель проходит валидацию, то в базе ищем логин
            {

                user = db.Users.FirstOrDefault(x => x.Login == registerModel.Login);
                if (user == null)//если такого пользователя нет, то создаём его
                {
                    user = new User
                    {
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName,
                        MiddleName = registerModel.MiddleName,
                        Email = registerModel.Email,
                        Password = registerModel.Password,
                        Phone = registerModel.Phone,
                        Login = registerModel.Login
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    //после сохранения получаем его
                    user = db.Users.Where
                        (x => x.Login == registerModel.Login && x.Password == registerModel.Password).FirstOrDefault();
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(registerModel.Login, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такой пользователь уже существует");
                }

            }
            return View(registerModel);
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
            // Выводим все дома, которые добавлял пользователь
            //_______________________________________________________________
            User user = (from u in db.Users
                         where u.Login == User.Identity.Name
                         select u).FirstOrDefault();

            IQueryable<House> house = from h in db.Houses
                                      where h.UserId == user.Id
                                      select h;

            List<House> hou = new List<House>(house);
            ViewBag.house = hou;
            //Выводим дома, по которым пришла бронь 
            //_______________________________________________________________

            IQueryable<Reservation> reserv = from r in db.Reservations
                         .Include(u => u.User)
                                             where r.House.UserId == user.Id
                                             select r;
            List<Reservation> res = new List<Reservation>(reserv);
            ViewBag.reserv = res;
            //Выводим дома, по которым оставлен отзыв 
            //_______________________________________________________________
            IQueryable<Review> comment = from c in db.Reviews
                          .Include(x => x.User).Include(x => x.House)
                                         where c.House.UserId == user.Id
                                         select c;

            List<Review> rew = new List<Review>(comment);
            ViewBag.rew = rew;

            return View();
        }
        #endregion        

        #region Редактирование моих домов
        public ActionResult EditMyHouse(int? id)
        {

            if (User.Identity.IsAuthenticated)
            {
                House house = db.Houses.Find(id);

                User user = (from u in db.Users
                             where u.Login == User.Identity.Name
                             select u).FirstOrDefault();
                //Вывод забронированных дат по выбранному дому
                IQueryable<Reservation> reserv = from r in db.Reservations
                                                .Include(x => x.User)
                                                 where r.House.UserId == user.Id && r.HouseId == house.Id
                                                 select r;

                List<Reservation> res = new List<Reservation>(reserv);
                ViewBag.reserv = res;

                //Вывод комментариев по выбранному дому
                IQueryable<Review> comment = from c in db.Reviews
                                             .Include(x => x.User).Include(x => x.House)
                                             where c.House.UserId == user.Id && c.HouseId == house.Id
                                             select c;

                List<Review> rew = new List<Review>(comment);
                ViewBag.rew = rew;

                return View(house);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyHouse(House house)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (ModelState.IsValid)
            {
                db.Entry(house).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(house);
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
                         where a.UserId == user.Id
                         select a;

            List<Reservation> res = new List<Reservation>(reserv);
            ViewBag.reserv = res;

            //var house = from h in db.Houses
            //            where h.
            //            select h;


            //var hou = new List<House>(house);
            //ViewBag.house = hou;
            return View();
        }
        #endregion
    }
}