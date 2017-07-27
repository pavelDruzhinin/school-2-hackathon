namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using RosCottedge.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<RosCottedge.Models.SiteContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RosCottedge.Models.SiteContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            context.Roles.AddOrUpdate(x => x.Id,
                new RosCottedge.Models.Role() {Id = 1, Name = "admin"},
                new RosCottedge.Models.Role() {Id = 2, Name = "user"}
            );

            context.Users.AddOrUpdate(x => x.Id,
                new User()
                {
                    Id=1,
                    Email = "admin@mail.ru",
                    FirstName = "admin",
                    LastName = "admin",
                    MiddleName = "admin",
                    Login = "admin",
                    Phone = "+79000000000",
                    Password = "admin",
                    OldPassword = "admin",
                    RegistrationDate = DateTime.Now,
                    RoleId = 1,
                    Avatar = "/Content/img/zlad.jpg"
                }
            );
        }
    }
}
