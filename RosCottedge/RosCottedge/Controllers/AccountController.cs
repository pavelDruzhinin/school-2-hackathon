using RosCottedge.Models;
using RosCottedge.Models.Login_register;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}