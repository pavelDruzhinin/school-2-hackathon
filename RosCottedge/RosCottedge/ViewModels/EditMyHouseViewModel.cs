using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.ViewModels
{
    public class EditMyHouseViewModel
    {
        public IQueryable<Review> Reviews { get; set; }
        public IQueryable<Reservation> Reservations { get; set; }
        public IQueryable<Picture> Pictures { get; set; }
        public IQueryable<ReservDelNotice> ReservDelNotices { get; set; }
        public House House { get; set; }
        public User User { get; set; }
    }
}