using RosCottedge.Models;
using RosCottedge.Models.Login_register;
using RosCottedge.ViewModels;
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
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Profile");

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
            if (User.Identity.IsAuthenticated )
                return new HttpStatusCodeResult(HttpStatusCode.Found);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register registerModel)
        {
            if (User.Identity.IsAuthenticated )
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
                    
                        FormsAuthentication.SetAuthCookie(registerModel.Login, true);
                        return RedirectToAction("Index", "Home");
                    
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
                Reservation = from r in db.Reservations
                                .Include(u => u.User)
                              where r.House.UserId == user.Id
                              select r,

                //Выводим дома, по которым оставлен отзыв 
                Review = from c in db.Reviews
                         .Include(x => x.User).Include(x => x.House)
                         where c.House.UserId == user.Id
                         select c

            };

            return View(myHouseModel);
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
                if (house.UserId != user.Id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                EditMyHouseViewModel viewModel = new EditMyHouseViewModel()
                {
                    House = house,

                    //Вывод забронированных дат по выбранному дому
                    Reservations = from r in db.Reservations
                                                .Include(x => x.User)
                                   where r.House.UserId == user.Id && r.HouseId == house.Id
                                   select r,
                    //Вывод комментариев по выбранному дому
                    Reviews = from c in db.Reviews
                                             .Include(x => x.User).Include(x => x.House)
                              where c.House.UserId == user.Id && c.HouseId == house.Id
                              select c

                };

                return View(viewModel);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
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
          
                editedHouse.Name = house.Name;
                editedHouse.Price = house.Price;
                editedHouse.NumberOfPersons = house.Price;
                editedHouse.Region = house.Region;
                editedHouse.Locality = house.Locality;
                editedHouse.Area = house.Area;
                editedHouse.HouseNumber = house.HouseNumber;
                editedHouse.Description = house.Description;
                editedHouse.Food = house.Food;
                editedHouse.Transfer = house.Transfer;
                editedHouse.ServicesIncluded = house.ServicesIncluded;
                editedHouse.AdditionalServices = house.AdditionalServices;
                editedHouse.Accomodations = house.Accomodations;
                editedHouse.BookingConditions = house.BookingConditions;
  
                db.SaveChanges();
                return RedirectToAction("MyHouse");
            }
            return RedirectToAction("EditMyHouse", "Account", new { id = house.Id });
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


            return View(reserv);
        }
        #endregion
    }
}