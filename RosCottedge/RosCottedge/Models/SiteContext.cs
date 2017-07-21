namespace RosCottedge.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SiteContext : DbContext
    {   
        public SiteContext()
            : base("name=SiteContext")
        {}
        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ReservDelNotice> ReservDelNotices { get; set; }
    }
}