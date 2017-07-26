using RosCottedge.Models;
using System;
using System.Linq;
using System.Web.Security;

namespace RosCottedge.Providers
{
    //Реализуем наш провайдер, чтобы самостоятельно обрабатывать данные о пользователе
    public class CustomRoleProvider : RoleProvider
    {
        //Теперь при обращении к User.Identity.IsAuthenticated мы будем вызывать метод GetRolesForUser
        public override string[] GetRolesForUser(string username)
        {
            string[] role = new string[] { };
            using (var db = new SiteContext())
            {
                // Получаем пользователя
                var user = db.Users.FirstOrDefault(u => u.Login == username);
                if (user != null)
                {
                    // получаем роль
                    var userRole = db.Roles.Find(user.RoleId);
                    if (userRole != null)
                        role = new string[] { userRole.Name };
                }
            }
            return role;
        }

        //Проверка Роли пользователя
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            // Находим пользователя
            using (var db = new SiteContext())
            {
                // Получаем пользователя
                var user = db.Users.FirstOrDefault(u => u.Email == username);
                if (user != null)
                {
                    // получаем роль
                    var userRole = db.Roles.Find(user.RoleId);
                    //сравниваем
                    if (userRole != null && userRole.Name == roleName)
                        outputResult = true;
                }
            }
            return outputResult;
        }

        #region

        public override void CreateRole(string roleName)
        {
            Role newRole = new Role() { Name = roleName };
            var db = new SiteContext();
            db.Roles.Add(newRole);
            db.SaveChanges();
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}