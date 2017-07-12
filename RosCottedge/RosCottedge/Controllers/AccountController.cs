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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register registerModel)
        {
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
    }
}