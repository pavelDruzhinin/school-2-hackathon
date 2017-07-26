namespace RosCottedge.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using RosCottedge.Providers;

    public class SiteContext : DbContext
    {
        static SiteContext()
        {
            Database.SetInitializer(new DefaultUser());
        }
        public SiteContext()
            : base("name=SiteContext")
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ReservDelNotice> ReservDelNotices { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
    class DefaultUser : CreateDatabaseIfNotExists<SiteContext>
    {
        protected override void Seed(SiteContext db)
        {
            Role role1 = new Role { Id = 1, Name = "admin" };
            Role role2 = new Role { Id = 2, Name = "user" };
            db.Roles.Add(role1);
            db.Roles.Add(role2);

            User admin = new User
            {
                Email = "admin@mail.ru",
                FirstName = "admin",
                LastName = "admin",
                MiddleName = "admin",
                Login = "admin",
                Phone = "+7900000000",
                Password = "admin",
                OldPassword = "admin",
                RegistrationDate = DateTime.Now,
                RoleId = 1,
                Avatar = "/Content/img/zlad.jpg"
            };
            db.Users.Add(admin);
            db.SaveChanges();
        }
    }
}